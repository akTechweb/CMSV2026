using Microsoft.AspNetCore.Mvc;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/pharmacist/audit-logs")]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _service;
        public AuditLogController(IAuditLogService service) => _service = service;

        private int PharmacistId => HttpContext.Session.GetInt32("PharmacistId") ?? 0;

        // GET: api/pharmacist/audit-logs?fromDate=&toDate=
        [HttpGet]
        public async Task<IActionResult> List(DateTime? fromDate, DateTime? toDate)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            var logs = await _service.GetAuditLogs(fromDate, toDate);
            return Ok(new { fromDate, toDate, logs });
        }
    }
}
