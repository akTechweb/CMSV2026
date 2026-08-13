using Microsoft.AspNetCore.Mvc;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/pharmacist")]
    public class PharmacyDashboardController : ControllerBase
    {
        private readonly IPharmacyDashboardService _dashboardService;
        public PharmacyDashboardController(IPharmacyDashboardService dashboardService) => _dashboardService = dashboardService;

        private int PharmacistId => HttpContext.Session.GetInt32("PharmacistId") ?? 0;

        // GET: api/pharmacist/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> Index()
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            var dashboard = await _dashboardService.GetDashboardData();
            return Ok(dashboard);
        }
    }
}
