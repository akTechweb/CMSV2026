using Microsoft.AspNetCore.Mvc;
using InfinityCoderzz_CMSV2026.Models;
using InfinityCoderzz_CMSV2026.Services;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _svc;
        public DoctorController(IDoctorService svc) => _svc = svc;

        private int DoctorId => HttpContext.Session.GetInt32("DoctorId") ?? 0;

        // POST: api/doctor/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var session = await _svc.AuthenticateDoctorAsync(model);

            if (session != null)
            {
                HttpContext.Session.SetInt32("DoctorId", session.DoctorId);
                HttpContext.Session.SetInt32("UserId", session.UserId);
                HttpContext.Session.SetString("FullName", session.FullName ?? "Doctor");
                HttpContext.Session.SetString("RoleName", session.RoleName ?? "Doctor");
                return Ok(session);
            }

            return Unauthorized(new { message = "Invalid username or password." });
        }

        // POST: api/doctor/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out." });
        }

        // GET: api/doctor/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            if (DoctorId == 0) return Unauthorized(new { message = "Not logged in." });
            var stats = await _svc.GetDashboardStatsAsync(DoctorId);
            string doctorName = HttpContext.Session.GetString("FullName") ?? "Doctor";
            return Ok(new { doctorName, stats });
        }

        // GET: api/doctor/appointments?targetDay=today
        [HttpGet("appointments")]
        public async Task<IActionResult> ViewAppointments(string targetDay = "today")
        {
            if (DoctorId == 0) return Unauthorized(new { message = "Not logged in." });

            List<AppointmentViewModel> list = targetDay == "tomorrow"
                ? await _svc.GetTomorrowAppointmentsAsync(DoctorId)
                : await _svc.GetTodaysAppointmentsAsync(DoctorId);

            return Ok(new { currentSelection = targetDay, appointments = list });
        }

        // GET: api/doctor/consultation/setup?appointmentId=
        [HttpGet("consultation/setup")]
        public async Task<IActionResult> StartConsultation(int appointmentId)
        {
            if (DoctorId == 0) return Unauthorized(new { message = "Not logged in." });
            var vm = await _svc.GetConsultationSetupDataAsync(appointmentId);
            return Ok(vm);
        }

        // POST: api/doctor/consultation
        // Submits the consultation and returns the final summary document directly
        // (the original MVC action stashed it in TempData and redirected to a
        // separate summary page — the API returns it in one call instead).
        [HttpPost("consultation")]
        public async Task<IActionResult> SubmitConsultation([FromBody] ConsultationSetupViewModel model)
        {
            if (DoctorId == 0) return Unauthorized(new { message = "Not logged in." });

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var summary = await _svc.SaveFullConsultationAsync(model, DoctorId);
            return Ok(summary);
        }

        // GET: api/doctor/patients/search?searchKeyword=
        [HttpGet("patients/search")]
        public async Task<IActionResult> HistoryAndReports(string searchKeyword = "")
        {
            if (DoctorId == 0) return Unauthorized(new { message = "Not logged in." });
            var list = await _svc.SearchPatientsAsync(searchKeyword);
            return Ok(new { searchKeyword, results = list });
        }

        // GET: api/doctor/patients/{mmrCode}/report
        [HttpGet("patients/{mmrCode}/report")]
        public async Task<IActionResult> GetPatientFullReport(string mmrCode)
        {
            if (DoctorId == 0) return Unauthorized(new { message = "Not logged in." });
            var report = await _svc.GetPatientFullReportAsync(mmrCode);
            return Ok(report);
        }
    }
}
