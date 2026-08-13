using InfinityCoderzz_CMSV2026.Models;
using InfinityCoderzzz_CMSV2026.Services;
using Microsoft.AspNetCore.Mvc;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientVisitsController : ControllerBase
    {
        private readonly IPatientVisitService _patientVisitService;

        public PatientVisitsController(IPatientVisitService patientVisitService)
        {
            _patientVisitService = patientVisitService;
        }

        // GET: api/patientvisits
        [HttpGet]
        public ActionResult<List<PatientVisit>> GetAll()
        {
            List<PatientVisit> visits = _patientVisitService.GetAllPatientVisits();
            return Ok(visits);
        }

        // GET: api/patientvisits/5
        [HttpGet("{id}")]
        public ActionResult<PatientVisit> GetById(int id)
        {
            PatientVisit visit = _patientVisitService.GetPatientVisitById(id);

            if (visit == null)
            {
                return NotFound();
            }

            return Ok(visit);
        }
    }
}
