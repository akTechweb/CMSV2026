using Microsoft.AspNetCore.Mvc;
using InfinityCoderzz_CMSV2026.Models;
using InfinityCoderzz_CMSV2026.Services;
using System.Text.RegularExpressions;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabTechnicianController : ControllerBase
    {
        private readonly ILabTechnicianService _svc;
        public LabTechnicianController(ILabTechnicianService svc) => _svc = svc;

        // A numeric result must start with a number, optionally followed by a
        // unit (e.g. "135", "98.6", "5.7%", "135 mg/dL"). Mirrors the same
        // rule enforced client-side in the Angular result entry form.
        private static readonly Regex NumericResultPattern =
            new Regex(@"^-?\d+(\.\d+)?\s*[a-zA-Z%/µ]*$", RegexOptions.Compiled);

        // Any digit at all means the normal range is numeric, even inside
        // worded text like "Less than 140 mg/dL". Purely descriptive ranges
        // such as "Negative" or "Positive/Negative" have no digits and are
        // left unconstrained.
        private static readonly Regex RangeHasDigit = new Regex(@"\d", RegexOptions.Compiled);

        // Hardcoded technician id (login handled by the shared/common module).
        // LabTechnicians table has a seeded row with TechnicianId = 1 (labtechadmin).
        private const int TechnicianId = 1;

        // GET: api/labtechnician/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var stats = await _svc.GetDashboardStatsAsync();
            return Ok(new { technicianName = "Lab Admin", stats });
        }

        // GET: api/labtechnician/pending-tests?searchMMR=
        [HttpGet("pending-tests")]
        public async Task<IActionResult> PendingTests(string searchMMR = "")
        {
            var list = await _svc.GetPendingLabRequestsAsync(searchMMR);
            return Ok(new { searchMMR, results = list });
        }

        // GET: api/labtechnician/results/5
        [HttpGet("results/{requestItemId}")]
        public async Task<IActionResult> GetResultEntryForm(int requestItemId)
        {
            var vm = await _svc.GetLabRequestItemDetailsAsync(requestItemId);
            if (vm == null) return NotFound();

            if (vm.Status == "Completed")
            {
                return Ok(new { message = "Result for this test has already been entered.", data = vm });
            }
            return Ok(vm);
        }

        // POST: api/labtechnician/results
        [HttpPost("results")]
        public async Task<IActionResult> EnterResult([FromBody] EnterLabResultViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(kv => kv.Value!.Errors.Select(e => $"{kv.Key}: {e.ErrorMessage}"))
                    .ToList();

                return BadRequest(new { message = "Validation failed — please check the form.", errors });
            }

            // Trim free-text fields before validating/persisting them.
            model.ResultValue = model.ResultValue?.Trim();
            model.Observation = model.Observation?.Trim();
            model.Remarks = model.Remarks?.Trim();

            if (string.IsNullOrWhiteSpace(model.ResultValue))
            {
                return BadRequest(new { message = "Validation failed — please check the form.", errors = new[] { "ResultValue: Result value is required" } });
            }

            // Re-confirm the request item exists and hasn't already had a result
            // entered, so a stale form (or a direct API call) can't create a
            // duplicate/orphaned result.
            var existing = await _svc.GetLabRequestItemDetailsAsync(model.RequestItemId);
            if (existing == null)
            {
                return NotFound(new { message = "This test request could not be found." });
            }
            if (existing.Status == "Completed")
            {
                return Conflict(new { message = "Result for this test has already been entered." });
            }

            if (RangeHasDigit.IsMatch(existing.NormalRange ?? string.Empty) &&
                !NumericResultPattern.IsMatch(model.ResultValue))
            {
                return BadRequest(new
                {
                    message = "Validation failed — please check the form.",
                    errors = new[]
                    {
                        $"ResultValue: This test's normal range ({existing.NormalRange}) is numeric — the result must start with a number (e.g. \"135\" or \"135 mg/dL\")."
                    }
                });
            }

            try
            {
                // Save result via stored procedure usp_AddLabResult
                int resultId = await _svc.SaveLabResultAsync(model, TechnicianId);

                if (resultId <= 0)
                {
                    return StatusCode(500, new
                    {
                        message = "Database error: Result was not saved. " +
                            "Please verify that usp_AddLabResult exists and all required " +
                            "tables (LabResults, LabRequestItems, LabRequests) are present."
                    });
                }

                // Auto-send the result to the assigned doctor's email.
                // Email failure does NOT roll back the saved result.
                bool emailSent = await _svc.SendResultToDoctorAsync(resultId);

                return Ok(new
                {
                    resultId,
                    message = emailSent
                        ? "Result saved successfully and emailed to the consulting doctor."
                        : "Result saved successfully. (Email could not be sent — check SMTP settings.)"
                });
            }
            catch (Microsoft.Data.SqlClient.SqlException sqlEx)
            {
                return StatusCode(500, new
                {
                    message = $"SQL Error {sqlEx.Number}: {sqlEx.Message} — " +
                        "Check that usp_AddLabResult exists and all referenced columns/tables exist."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Unexpected error: {ex.Message}" });
            }
        }

        // GET: api/labtechnician/reports?searchMMR=
        [HttpGet("reports")]
        public async Task<IActionResult> ReportsDashboard(string searchMMR = "")
        {
            var list = await _svc.GetCompletedLabReportsAsync(searchMMR);
            return Ok(new { searchMMR, results = list });
        }

        // GET: api/labtechnician/results/5/detail
        // Full result detail (used by the original PDF/print view).
        [HttpGet("results/{resultId}/detail")]
        public async Task<IActionResult> GetResultDetail(int resultId)
        {
            var r = await _svc.GetLabResultDetailsAsync(resultId);
            if (r == null) return NotFound();
            return Ok(r);
        }

        // POST: api/labtechnician/results/5/resend-email?searchMMR=
        [HttpPost("results/{resultId}/resend-email")]
        public async Task<IActionResult> ResendEmail(int resultId, string searchMMR = "")
        {
            bool sent = await _svc.SendResultToDoctorAsync(resultId);

            return Ok(new
            {
                success = sent,
                message = sent
                    ? "Report emailed to doctor successfully."
                    : "Failed to send email. Please check the doctor's email address and SMTP settings."
            });
        }

        // GET: api/labtechnician/billing?searchMMR=
        [HttpGet("billing")]
        public async Task<IActionResult> BillingDashboard(string searchMMR = "")
        {
            var unbilled = await _svc.GetUnbilledLabRequestsAsync(searchMMR);
            var bills = await _svc.GetLabBillsAsync(searchMMR);
            return Ok(new { searchMMR, unbilled, bills });
        }

        public class GenerateBillRequest
        {
            public int RequestId { get; set; }
        }

        // POST: api/labtechnician/billing/generate?searchMMR=
        [HttpPost("billing/generate")]
        public async Task<IActionResult> GenerateBill([FromBody] GenerateBillRequest request, string searchMMR = "")
        {
            try
            {
                int billId = await _svc.GenerateLabBillAsync(request.RequestId, TechnicianId);
                return Ok(new { billId, message = $"Bill #{billId} generated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to generate bill: {ex.Message}" });
            }
        }

        public class UpdateBillPaymentRequest
        {
            public string PaymentStatus { get; set; } = string.Empty;
        }

        // PUT: api/labtechnician/billing/5/payment-status
        [HttpPut("billing/{billId}/payment-status")]
        public async Task<IActionResult> UpdateBillPayment(int billId, [FromBody] UpdateBillPaymentRequest request)
        {
            await _svc.UpdateLabBillPaymentStatusAsync(billId, request.PaymentStatus);
            return Ok(new { message = "Payment status updated." });
        }

        // GET: api/labtechnician/billing/5
        [HttpGet("billing/{billId}")]
        public async Task<IActionResult> PrintBill(int billId)
        {
            var vm = await _svc.GetLabBillDetailsAsync(billId);
            if (vm == null || vm.Bill == null) return NotFound();
            return Ok(vm);
        }

        // GET: api/labtechnician/billing/5/pdf
        // Returns the actual generated PDF file (via QuestPDF, in the service layer).
        [HttpGet("billing/{billId}/pdf")]
        public async Task<IActionResult> DownloadBillPdf(int billId)
        {
            try
            {
                var vm = await _svc.GetLabBillDetailsAsync(billId);
                if (vm == null || vm.Bill == null) return NotFound();

                var pdfStream = await _svc.GenerateBillPdfAsync(billId);
                string filename = $"Bill_{vm.Bill.MMRCode}_{vm.Bill.BillId}.pdf";
                return File(pdfStream, "application/pdf", filename);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to generate PDF: {ex.Message}" });
            }
        }

        // GET: api/labtechnician/patients/search?term=
        [HttpGet("patients/search")]
        public async Task<IActionResult> SearchPatientByMMR(string term)
        {
            var list = await _svc.SearchPatientByMMRAsync(term);
            return Ok(list);
        }
    }
}
