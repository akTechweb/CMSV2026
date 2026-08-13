using InfinityCoderzzz_CMSV2026.Services;
using Microsoft.AspNetCore.Mvc;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceptionistsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IBillService _billService;

        public ReceptionistsController(
            IPatientService patientService,
            IAppointmentService appointmentService,
            IBillService billService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _billService = billService;
        }

        // GET: api/receptionists/dashboard
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            string userName = HttpContext.Session.GetString("FullName") ?? "Receptionist";

            var patients = _patientService.GetAllPatients();

            var todayAppointments = _appointmentService.GetAppointmentsByFilter(
                null, null, null, DateTime.Today, DateTime.Today);

            var tomorrowAppointments = _appointmentService.GetAppointmentsByFilter(
                null, null, null, DateTime.Today.AddDays(1), DateTime.Today.AddDays(1));

            var todayBills = _billService.GetAllBills()
                .Where(b => b.BillDate.HasValue &&
                            b.BillDate.Value.Date == DateTime.Today &&
                            b.Status == "Paid")
                .ToList();

            var recentPatients = patients
                .OrderByDescending(p => p.PatientId)
                .Take(5)
                .ToList();

            return Ok(new
            {
                userName,
                totalPatients = patients.Count,
                todayAppointments = todayAppointments.Count,
                tomorrowAppointments = tomorrowAppointments.Count,
                todayCollection = todayBills.Sum(b => b.TotalAmount),
                recentPatients
            });
        }

        // GET: api/receptionists/search-patient?keyword=MMR000001
        [HttpGet("search-patient")]
        public IActionResult SearchPatient(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new
                {
                    message = "Please enter MMR Number or Mobile Number"
                });
            }

            var patients = _patientService.SearchPatients(keyword);

            if (patients == null || patients.Count == 0)
            {
                return Ok(new
                {
                    found = false,
                    message = "Patient not found. Please register new patient."
                });
            }

            return Ok(new
            {
                found = true,
                patients
            });
        }
    }
}
