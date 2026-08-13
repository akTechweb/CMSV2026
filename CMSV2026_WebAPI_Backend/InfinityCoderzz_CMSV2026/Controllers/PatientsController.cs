using InfinityCoderzz_CMSV2026.Models;
using InfinityCoderzzz_CMSV2026.Services;
using Microsoft.AspNetCore.Mvc;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: api/patients/next-code
        [HttpGet("next-code")]
        public ActionResult<string> GetNextPatientCode()
        {
            return Ok(new { patientCode = _patientService.GetNextPatientCode() });
        }

        // POST: api/patients
        [HttpPost]
        public IActionResult Create([FromBody] Patient patient)
        {
            try
            {
                ModelState.Remove("PatientCode");
                ModelState.Remove("BloodGroup");

                if (string.IsNullOrWhiteSpace(patient.BloodGroup))
                {
                    patient.BloodGroup = "Unknown";
                }

                ValidatePatient(patient);

                if (!ModelState.IsValid)
                {
                    string errors = string.Join(" | ",
                        ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    return BadRequest(new { message = errors });
                }

                _patientService.RegisterPatient(patient);

                return CreatedAtAction(nameof(GetById), new { id = patient.PatientId }, new
                {
                    patientId = patient.PatientId,
                    patientCode = patient.PatientCode,
                    fullName = patient.FullName,
                    mobile = patient.MobileNumber,
                    dob = patient.DOB?.ToString("dd-MM-yyyy") ?? "",
                    age = patient.Age?.ToString() ?? "",
                    gender = patient.Gender
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Registration failed: " + ex.Message });
            }
        }

        // GET: api/patients/search?searchBy=MMR&searchText=xxx
        [HttpGet("search")]
        public ActionResult<List<Patient>> Search(string? searchBy, string? searchText)
        {
            List<Patient> results = new();

            searchBy = string.IsNullOrWhiteSpace(searchBy) ? "MMR" : searchBy;
            searchText = searchText?.Trim();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                results = _patientService.SearchPatients(searchText);
            }

            return Ok(new
            {
                searchBy,
                searchText,
                patientNotFound = !string.IsNullOrWhiteSpace(searchText) && !results.Any(),
                results
            });
        }

        // GET: api/patients
        [HttpGet]
        public ActionResult<List<Patient>> GetAll()
        {
            return Ok(_patientService.GetAllPatients());
        }

        // GET: api/patients/5
        [HttpGet("{id}")]
        public ActionResult<Patient> GetById(int id)
        {
            Patient? patient = _patientService.GetPatientById(id);

            if (patient == null)
                return NotFound();

            return Ok(patient);
        }

        // PUT: api/patients/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Patient patient)
        {
            if (id != patient.PatientId)
                return BadRequest(new { message = "Route id does not match PatientId in body." });

            ValidatePatient(patient);

            if (!ModelState.IsValid)
            {
                string errors = string.Join(" | ",
                    ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(new { message = errors });
            }

            _patientService.UpdatePatient(patient);

            return Ok(new { message = "Patient details edited successfully.", patientId = patient.PatientId });
        }

        private void ValidatePatient(Patient patient)
        {
            ModelState.Remove("PatientCode");
            ModelState.Remove("RegistrationDate");
            ModelState.Remove("IsActive");

            if (string.IsNullOrWhiteSpace(patient.BloodGroup))
            {
                patient.BloodGroup = "Unknown";
                ModelState.Remove("BloodGroup");
            }

            if (patient.DOB.HasValue)
            {
                DateTime today = DateTime.Today;

                if (patient.DOB.Value.Date > today)
                {
                    ModelState.AddModelError("DOB", "Date of birth cannot be a future date.");
                }
                else
                {
                    int age = today.Year - patient.DOB.Value.Year;

                    if (patient.DOB.Value.Date > today.AddYears(-age))
                        age--;

                    if (age < 0 || age > 100)
                    {
                        ModelState.AddModelError("DOB", "Age must be between 0 and 100 years.");
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(patient.MobileNumber) &&
                !System.Text.RegularExpressions.Regex.IsMatch(patient.MobileNumber, @"^[6-9][0-9]{9}$"))
            {
                ModelState.AddModelError("MobileNumber", "Mobile number must be 10 digits and start with 6, 7, 8, or 9.");
            }

            if (!string.IsNullOrWhiteSpace(patient.EmergencyContactNumber) &&
                !System.Text.RegularExpressions.Regex.IsMatch(patient.EmergencyContactNumber, @"^[6-9][0-9]{9}$"))
            {
                ModelState.AddModelError("EmergencyContactNumber", "Emergency contact must be 10 digits and start with 6, 7, 8, or 9.");
            }
        }
    }
}
