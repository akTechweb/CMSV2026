using Microsoft.AspNetCore.Mvc;
using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/pharmacist/medicines")]
    public class MedicinesController : ControllerBase
    {
        private readonly IMedicineService _medicineService;
        public MedicinesController(IMedicineService medicineService) => _medicineService = medicineService;

        private int PharmacistId => HttpContext.Session.GetInt32("PharmacistId") ?? 0;

        // GET: api/pharmacist/medicines?searchTerm=
        [HttpGet]
        public async Task<IActionResult> List(string? searchTerm)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            IEnumerable<Medicine> medicines = !string.IsNullOrWhiteSpace(searchTerm)
                ? await _medicineService.SearchMedicine(searchTerm)
                : await _medicineService.GetAllMedicines();

            return Ok(new { searchTerm, medicines });
        }

        // GET: api/pharmacist/medicines/new
        // Metadata needed to render a "create medicine" form (categories, manufacturers, auto code).
        [HttpGet("new")]
        public async Task<IActionResult> NewMedicineMeta()
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            return Ok(new
            {
                categories       = await _medicineService.GetAllCategories(),
                manufacturers    = await _medicineService.GetAllManufacturers(),
                nextMedicineCode = await _medicineService.GenerateNextMedicineCode()
            });
        }

        // GET: api/pharmacist/medicines/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            var medicine = await _medicineService.GetMedicineById(id);
            if (medicine == null) return NotFound();

            return Ok(new
            {
                medicine,
                categories    = await _medicineService.GetAllCategories(),
                manufacturers = await _medicineService.GetAllManufacturers()
            });
        }

        // POST: api/pharmacist/medicines
        // Returns 201 Created with a Location header pointing to the new resource.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Medicine medicine)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            try
            {
                if (string.IsNullOrWhiteSpace(medicine.MedicineCode))
                {
                    medicine.MedicineCode = await _medicineService.GenerateNextMedicineCode();
                    ModelState.Remove(nameof(Medicine.MedicineCode));
                }
                else
                {
                    medicine.MedicineCode = medicine.MedicineCode.Trim();

                    if (!await _medicineService.IsMedicineCodeUnique(medicine.MedicineCode))
                    {
                        ModelState.AddModelError(
                            nameof(Medicine.MedicineCode),
                            "This medicine code is already in use. Leave it blank to auto-generate.");
                    }
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _medicineService.AddMedicine(medicine);

                // 201 Created — Location header points to GET /api/pharmacist/medicines/{id}
                return CreatedAtAction(nameof(GetById), new { id = medicine.MedicineId },
                    new { message = "Medicine added successfully.", medicine });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to add medicine. " + ex.Message });
            }
        }

        // PUT: api/pharmacist/medicines/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Medicine medicine)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });
            if (!ModelState.IsValid) return BadRequest(ModelState);

            medicine.MedicineId = id;
            await _medicineService.UpdateMedicine(medicine);
            return Ok(new { message = "Medicine updated successfully.", medicine });
        }

        // POST: api/pharmacist/medicines/{id}/disable
        [HttpPost("{id:int}/disable")]
        public async Task<IActionResult> Disable(int id)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            await _medicineService.DisableMedicine(id);
            return Ok(new { message = "Medicine disabled." });
        }
    }
}
