using Microsoft.AspNetCore.Mvc;
using InfinityCoderzz_CMSV2026.DTOs.Pharmacy;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/pharmacist/prescriptions")]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
            => _prescriptionService = prescriptionService;

        private int PharmacistId => HttpContext.Session.GetInt32("PharmacistId") ?? 0;

        // GET: api/pharmacist/prescriptions
        [HttpGet]
        public async Task<IActionResult> List()
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });
            return Ok(await _prescriptionService.GetAllPrescriptions());
        }

        // GET: api/pharmacist/prescriptions/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            var prescription = await _prescriptionService.GetPrescriptionById(id);
            if (prescription == null) return NotFound(new { message = $"Prescription {id} not found." });

            var items = await _prescriptionService.GetPrescriptionItems(id);
            return Ok(new { prescription, items });
        }

        // POST: api/pharmacist/prescriptions/{id}/dispense
        [HttpPost("{id:int}/dispense")]
        public async Task<IActionResult> MarkDispensed(int id)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            try
            {
                await _prescriptionService.UpdatePrescriptionStatus(id, "Dispensed");
                return Ok(new { message = "Prescription marked as Dispensed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Could not update prescription status. " + ex.Message });
            }
        }

        // PUT: api/pharmacist/prescriptions/{id}/status
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdatePrescriptionStatusRequest request)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(request.Status))
                return BadRequest(new { message = "Status cannot be empty." });

            try
            {
                await _prescriptionService.UpdatePrescriptionStatus(id, request.Status);
                return Ok(new { message = $"Prescription status updated to '{request.Status}'." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to update status. " + ex.Message });
            }
        }
    }
}
