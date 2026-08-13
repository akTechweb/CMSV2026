using Microsoft.AspNetCore.Mvc;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/pharmacist/inventory-logs")]
    public class InventoryLogController : ControllerBase
    {
        private readonly IInventoryLogService _service;
        public InventoryLogController(IInventoryLogService service) => _service = service;

        private int PharmacistId => HttpContext.Session.GetInt32("PharmacistId") ?? 0;

        // GET: api/pharmacist/inventory-logs
        [HttpGet]
        public async Task<IActionResult> List()
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });
            return Ok(await _service.GetInventoryLogs());
        }
    }
}
