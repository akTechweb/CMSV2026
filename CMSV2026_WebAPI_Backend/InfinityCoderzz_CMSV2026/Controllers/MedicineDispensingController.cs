using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using InfinityCoderzz_CMSV2026.DTOs.Pharmacy;
using InfinityCoderzz_CMSV2026.Helpers;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/pharmacist/dispensing")]
    public class MedicineDispensingController : ControllerBase
    {
        private readonly IMedicineDispensingService _service;

        public MedicineDispensingController(IMedicineDispensingService service) => _service = service;

        private int PharmacistId => HttpContext.Session.GetInt32("PharmacistId") ?? 0;

        // GET: api/pharmacist/dispensing
        [HttpGet]
        public async Task<IActionResult> PendingPrescriptions()
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });
            return Ok(await _service.GetDispensablePrescriptions());
        }

        // POST: api/pharmacist/dispensing
        [HttpPost]
        public async Task<IActionResult> DispenseAndBill([FromBody] DispensePrescriptionRequest request)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.PrescriptionId <= 0)
                return BadRequest(new { message = "Invalid prescription selected." });

            try
            {
                var result = await _service.DispenseAndBill(request.PrescriptionId, PharmacistId, request.Remarks);
                return Ok(new
                {
                    message = $"Prescription dispensed and bill {RefNo.Bill(result.BillId)} generated automatically. " +
                              $"Stock updated (Dispense {RefNo.Dispense(result.DispenseId)}).",
                    result
                });
            }
            catch (InvalidOperationException ex)
            {
                // Business rule violations raised in C# (no medicines, patient not resolved, etc.)
                return Conflict(new { message = "Failed to dispense prescription. " + ex.Message });
            }
            catch (SqlException ex) when (ex.Number == 50001)
            {
                // Insufficient stock — thrown by sp_DispenseMedicine via THROW 50001.
                return Conflict(new { message = "Failed to dispense prescription. " + ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to dispense prescription. " + ex.Message });
            }
        }


        // GET: api/pharmacist/dispensing/{prescriptionId}/stock-check
        [HttpGet("{prescriptionId:int}/stock-check")]
        public async Task<IActionResult> StockCheck(int prescriptionId)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });
            if (prescriptionId <= 0) return BadRequest(new { message = "Invalid prescription." });
            return Ok(await _service.CheckStock(prescriptionId));
        }

        // GET: api/pharmacist/dispensing/history
        [HttpGet("history")]
        public async Task<IActionResult> History()
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });
            return Ok(await _service.GetDispensingHistory());
        }

        // GET: api/pharmacist/dispensing/{dispenseId}/items
        [HttpGet("{dispenseId:int}/items")]
        public async Task<IActionResult> Items(int dispenseId)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });
            return Ok(await _service.GetDispensingItems(dispenseId));
        }
    }
}
