using InfinityCoderzz_CMSV2026.Models;
using InfinityCoderzzz_CMSV2026.Services;
using Microsoft.AspNetCore.Mvc;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceptionistReportsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IBillService _billService;

        public ReceptionistReportsController(
            IPatientService patientService,
            IAppointmentService appointmentService,
            IBillService billService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _billService = billService;
        }

        // GET: api/receptionistreports?reportType=&fromDate=&toDate=&doctorId=&departmentId=
        [HttpGet]
        public IActionResult Get(
            string? reportType,
            DateTime? fromDate,
            DateTime? toDate,
            int? doctorId,
            int? departmentId)
        {
            reportType = string.IsNullOrWhiteSpace(reportType)
                ? "Appointment"
                : reportType;

            DateTime startDate = fromDate ?? DateTime.Today;
            DateTime endDate = toDate ?? DateTime.Today;

            var doctors = _appointmentService.GetAllActiveDoctors();

            var departments = doctors
                .Where(d => !string.IsNullOrWhiteSpace(d.DepartmentName))
                .GroupBy(d => new { d.DepartmentId, d.DepartmentName })
                .Select(g => g.Key)
                .ToList();

            var appointments = _appointmentService.GetAppointmentsByFilter(
                departmentId,
                doctorId,
                null,
                startDate,
                endDate);

            var patients = _patientService.GetAllPatients()
                .Where(p =>
                    p.RegistrationDate.HasValue &&
                    p.RegistrationDate.Value.Date >= startDate.Date &&
                    p.RegistrationDate.Value.Date <= endDate.Date)
                .ToList();

            var bills = _billService.GetAllBills()
                .Where(b =>
                    b.BillDate.HasValue &&
                    b.BillDate.Value.Date >= startDate.Date &&
                    b.BillDate.Value.Date <= endDate.Date)
                .ToList();

            // Bills don't carry DoctorId/DepartmentId directly — trace each bill
            // back to the appointment it was generated from to apply the same
            // doctor/department filter the Appointment report uses. Without this,
            // "Billing" always showed every doctor's revenue regardless of filter.
            if (doctorId.HasValue || departmentId.HasValue)
            {
                var doctorDeptMap = doctors.ToDictionary(d => d.DoctorId, d => d.DepartmentId);
                var apptDoctorMap = _appointmentService
                    .GetAppointmentsByFilter(null, null, null, startDate, endDate)
                    .ToDictionary(a => a.AppointmentId, a => a.DoctorId);

                bills = bills.Where(b =>
                {
                    if (!b.AppointmentId.HasValue || !apptDoctorMap.TryGetValue(b.AppointmentId.Value, out int billDoctorId))
                        return false; // can't attribute this bill to a doctor, so exclude it while a doctor/department filter is active

                    if (doctorId.HasValue && billDoctorId != doctorId.Value)
                        return false;

                    if (departmentId.HasValue &&
                        (!doctorDeptMap.TryGetValue(billDoctorId, out int billDeptId) || billDeptId != departmentId.Value))
                        return false;

                    return true;
                }).ToList();
            }

            // Collection totals must reflect actual payments received, not just the
            // bill's overall Status — a "Pending" bill can still have a partial
            // payment recorded against it (the Create Bill flow allows this).
            decimal totalCollection = 0;
            decimal pendingCollection = 0;

            foreach (var b in bills)
            {
                Bill full = _billService.GetBillById(b.BillId);
                decimal paid = full?.Payments?
                    .Where(p => p.PaymentStatus == "Completed")
                    .Sum(p => p.Amount) ?? 0;

                totalCollection += paid;
                pendingCollection += Math.Max(0, b.TotalAmount - paid);
            }

            return Ok(new
            {
                reportType,
                fromDate = startDate.ToString("yyyy-MM-dd"),
                toDate = endDate.ToString("yyyy-MM-dd"),
                doctorId,
                departmentId,
                doctors,
                departments,
                appointments,
                patients,
                bills,
                summary = new
                {
                    totalAppointments = appointments.Count,
                    completedAppointments = appointments.Count(a => a.Status == "Completed"),
                    pendingAppointments = appointments.Count(a => a.Status == "Scheduled"),
                    cancelledAppointments = appointments.Count(a => a.Status == "Cancelled"),
                    totalRegistrations = patients.Count,
                    totalCollection,
                    pendingCollection
                }
            });
        }
    }
}
