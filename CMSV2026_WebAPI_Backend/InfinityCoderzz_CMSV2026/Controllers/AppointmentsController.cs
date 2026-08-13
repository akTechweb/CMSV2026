using InfinityCoderzz_CMSV2026.Models;
using InfinityCoderzzz_CMSV2026.Services;
using Microsoft.AspNetCore.Mvc;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;

        public AppointmentsController(
            IAppointmentService appointmentService,
            IPatientService patientService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
        }

        // GET: api/appointments?departmentId=&doctorId=&patientCode=&fromDate=&toDate=
        [HttpGet]
        public ActionResult<List<Appointment>> GetAll(
            int? departmentId,
            int? doctorId,
            string? patientCode,
            DateTime? fromDate,
            DateTime? toDate)
        {
            bool hasFilter =
                departmentId.HasValue ||
                doctorId.HasValue ||
                !string.IsNullOrWhiteSpace(patientCode) ||
                fromDate.HasValue ||
                toDate.HasValue;

            List<Appointment> appointments = hasFilter
                ? _appointmentService.GetAppointmentsByFilter(
                    departmentId,
                    doctorId,
                    patientCode,
                    fromDate,
                    toDate)
                : _appointmentService.GetAllAppointments();

            return Ok(appointments);
        }

        // GET: api/appointments/doctors
        // Active doctors, grouped by department - used to populate filters/dropdowns.
        [HttpGet("doctors")]
        public ActionResult<object> GetDoctorsAndDepartments()
        {
            var doctors = _appointmentService.GetAllActiveDoctors();

            var departments = doctors
                .Where(d => !string.IsNullOrWhiteSpace(d.DepartmentName))
                .GroupBy(d => new { d.DepartmentId, d.DepartmentName })
                .Select(g => g.Key)
                .ToList();

            return Ok(new { doctors, departments });
        }

        // GET: api/appointments/create-data?patientId=
        // Data needed to build a "new appointment" form (doctor list, patient list, selected patient).
        [HttpGet("create-data")]
        public ActionResult<object> GetCreateData(int? patientId)
        {
            var doctors = _appointmentService.GetAllActiveDoctors();
            var patients = _patientService.GetAllPatients();
            Patient? selectedPatient = patientId.HasValue
                ? _patientService.GetPatientById(patientId.Value)
                : null;

            return Ok(new
            {
                doctors,
                patients,
                selectedPatient,
                defaultAppointmentDate = DateTime.Today
            });
        }

        // POST: api/appointments
        [HttpPost]
        public IActionResult Create([FromBody] Appointment appointment)
        {
            _appointmentService.BookAppointment(appointment, out string message);

            if (!string.IsNullOrWhiteSpace(message) &&
                message.ToUpper().Contains("SUCCESS"))
            {
                Appointment? savedAppointment =
                    _appointmentService.GetAppointmentById(appointment.AppointmentId);

                if (savedAppointment == null)
                {
                    return Ok(new
                    {
                        message = "Appointment was created, but summary details could not be loaded."
                    });
                }

                return CreatedAtAction(nameof(GetById), new { id = savedAppointment.AppointmentId }, savedAppointment);
            }

            return BadRequest(new { message });
        }

        // GET: api/appointments/booked-slots?doctorId=&appointmentDate=
        [HttpGet("booked-slots")]
        public ActionResult<List<string>> GetBookedSlots(int doctorId, DateTime appointmentDate)
        {
            var appointments = _appointmentService.GetAppointmentsByFilter(
                null,
                doctorId,
                null,
                appointmentDate,
                appointmentDate);

            var bookedSlots = appointments
                .Where(a => a.Status != "Cancelled")
                .Select(a => a.AppointmentTime.ToString(@"hh\:mm\:ss"))
                .ToList();

            return Ok(bookedSlots);
        }

        // GET: api/appointments/5
        [HttpGet("{id}")]
        public ActionResult<Appointment> GetById(int id)
        {
            Appointment? appointment = _appointmentService.GetAppointmentById(id);

            if (appointment == null)
                return NotFound();

            return Ok(appointment);
        }

        // POST: api/appointments/5/cancel
        [HttpPost("{id}/cancel")]
        public IActionResult Cancel(int id)
        {
            _appointmentService.CancelAppointment(id, out string message);

            if (!string.IsNullOrWhiteSpace(message) &&
                message.StartsWith("SUCCESS"))
            {
                return Ok(new { message = "Appointment cancelled." });
            }

            return BadRequest(new { message });
        }
    }
}
