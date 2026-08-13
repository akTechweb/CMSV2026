using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/pharmacist/medicine-stock")]
    public class MedicineStockController : ControllerBase
    {
        private readonly IMedicineStockService _medicineStockService;
        public MedicineStockController(IMedicineStockService medicineStockService) => _medicineStockService = medicineStockService;

        private int PharmacistId => HttpContext.Session.GetInt32("PharmacistId") ?? 0;

        // GET: api/pharmacist/medicine-stock
        [HttpGet]
        public async Task<IActionResult> List()
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });
            return Ok(await _medicineStockService.GetAllMedicineStock());
        }

        // GET: api/pharmacist/medicine-stock/new
        [HttpGet("new")]
        public async Task<IActionResult> NewStockMeta()
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });
            return Ok(new { medicines = await _medicineStockService.GetAllMedicines() });
        }

        // POST: api/pharmacist/medicine-stock
        // Returns 201 Created with a Location header pointing to the new stock record.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MedicineStock stock)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            stock.StockId     = 0;
            stock.BatchNumber = stock.BatchNumber?.Trim();

            // Selected medicine must exist and be active.
            if (!(await _medicineStockService.GetAllMedicines()).Any(m => m.MedicineId == stock.MedicineId))
                ModelState.AddModelError(nameof(stock.MedicineId), "Selected medicine is not valid or is inactive.");

            // Batch number format.
            if (!string.IsNullOrWhiteSpace(stock.BatchNumber)
                && !Regex.IsMatch(stock.BatchNumber, @"^[A-Za-z0-9/\-]+$"))
            {
                ModelState.AddModelError(nameof(stock.BatchNumber),
                    "Batch number may only contain letters, numbers, hyphen (-) and slash (/).");
            }

            // New stock cannot already be expired.
            if (stock.ExpiryDate != default && stock.ExpiryDate.Date <= DateTime.Today)
                ModelState.AddModelError(nameof(stock.ExpiryDate), "Expiry date must be a future date.");

            // Purchase price may not have more than 2 decimal places.
            if (stock.PurchasePrice.HasValue && HasMoreThanTwoDecimals(stock.PurchasePrice.Value))
                ModelState.AddModelError(nameof(stock.PurchasePrice), "Purchase price cannot have more than 2 decimal places.");

            // Batch number must be unique per medicine.
            if (stock.MedicineId > 0 && !string.IsNullOrWhiteSpace(stock.BatchNumber)
                && await _medicineStockService.BatchExists(stock.MedicineId, stock.BatchNumber, 0))
            {
                ModelState.AddModelError(nameof(stock.BatchNumber), "This batch number already exists for the selected medicine.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (stock.PurchaseDate == null)
                stock.PurchaseDate = DateTime.Today;

            try
            {
                await _medicineStockService.AddMedicineStock(stock);
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                ModelState.AddModelError(nameof(stock.BatchNumber), "This batch number already exists for the selected medicine.");
                return BadRequest(ModelState);
            }

            // 201 Created — Location header points to GET /api/pharmacist/medicine-stock/{id}
            return CreatedAtAction(nameof(GetById), new { id = stock.StockId },
                new { message = "Stock added successfully.", stock });
        }

        // GET: api/pharmacist/medicine-stock/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            var stock = await _medicineStockService.GetMedicineStockById(id);
            if (stock == null) return NotFound();
            return Ok(stock);
        }

        // PUT: api/pharmacist/medicine-stock/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] MedicineStock stock)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            stock.StockId     = id;
            stock.BatchNumber = stock.BatchNumber?.Trim();

            var existing = await _medicineStockService.GetMedicineStockById(stock.StockId);
            if (existing == null) return NotFound();

            // Batch must stay unique (excluding this row).
            if (stock.MedicineId > 0 && !string.IsNullOrWhiteSpace(stock.BatchNumber)
                && await _medicineStockService.BatchExists(stock.MedicineId, stock.BatchNumber, stock.StockId))
            {
                ModelState.AddModelError(nameof(stock.BatchNumber), "This batch number already exists for the selected medicine.");
            }

            if (stock.PurchasePrice.HasValue && HasMoreThanTwoDecimals(stock.PurchasePrice.Value))
                ModelState.AddModelError(nameof(stock.PurchasePrice), "Purchase price cannot have more than 2 decimal places.");

            if (stock.ExpiryDate != default && existing.PurchaseDate.HasValue
                && stock.ExpiryDate.Date <= existing.PurchaseDate.Value.Date)
            {
                ModelState.AddModelError(nameof(stock.ExpiryDate), "Expiry date must be after the purchase date.");
            }

            if (stock.ExpiryDate != default
                && stock.ExpiryDate.Date != existing.ExpiryDate.Date
                && stock.ExpiryDate.Date <= DateTime.Today)
            {
                ModelState.AddModelError(nameof(stock.ExpiryDate), "Expiry date cannot be changed to a past date.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _medicineStockService.UpdateMedicineStock(stock);
            return Ok(new { message = "Stock updated successfully.", stock });
        }

        // GET: api/pharmacist/medicine-stock/low-stock
        [HttpGet("low-stock")]
        public async Task<IActionResult> LowStock()
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });
            return Ok(await _medicineStockService.GetLowStockMedicines());
        }

        // GET: api/pharmacist/medicine-stock/expiring
        [HttpGet("expiring")]
        public async Task<IActionResult> ExpiringMedicines()
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });
            return Ok(await _medicineStockService.GetExpiringMedicines());
        }

        // GET: api/pharmacist/medicine-stock/expired
        [HttpGet("expired")]
        public async Task<IActionResult> ExpiredMedicines()
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });
            return Ok(await _medicineStockService.GetExpiredMedicines());
        }

        private static bool HasMoreThanTwoDecimals(decimal value)
            => value != decimal.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}
