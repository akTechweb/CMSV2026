using Microsoft.AspNetCore.Mvc;
using InfinityCoderzz_CMSV2026.DTOs.Pharmacy;
using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Pdf;
using QuestPDF.Fluent;


namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/pharmacist/bills")]
    public class BillController : ControllerBase
    {
        private readonly IPharmacyBillService _billService;

        public BillController(IPharmacyBillService billService) => _billService = billService;

        private int PharmacistId => HttpContext.Session.GetInt32("PharmacistId") ?? 0;

        // GET: api/pharmacist/bills
        [HttpGet]
        public async Task<IActionResult> List()
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });
            return Ok(await _billService.GetAllBills());
        }

        // GET: api/pharmacist/bills/new
        [HttpGet("new")]
        public async Task<IActionResult> NewBillMeta()
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            var patients  = await _billService.GetPatients();
            var medicines = await _billService.GetMedicinesForBilling();

            return Ok(new
            {
                patients  = patients.Select(p => new { p.PatientId, p.PatientCode, p.FullName, p.DisplayText }),
                medicines
            });
        }

        // POST: api/pharmacist/bills
        // Returns 201 Created with the new bill in the body.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBillViewModel model)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            if (model.BillItems == null || !model.BillItems.Any())
                return BadRequest(new { message = "Please add at least one medicine to the bill." });

            try
            {
                int billId = await _billService.CreateBill(model, PharmacistId);
                var bill   = await _billService.GetBillById(billId);

                // 201 Created — points to the Details action for the Location header.
                return CreatedAtAction(nameof(Details), new { id = billId },
                    new { message = "Bill created successfully.", bill });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create bill. " + ex.Message });
            }
        }

        // GET: api/pharmacist/bills/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            var bill = await _billService.GetBillById(id);
            if (bill == null) return NotFound(new { message = $"Bill {id} not found." });

            return Ok(new
            {
                bill,
                items            = await _billService.GetBillItems(id),
                prescriptionLink = await _billService.GetBillPrescriptionLink(id)
            });
        }

        // POST: api/pharmacist/bills/{id}/cancel
        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id, [FromBody] CancelBillRequest? request)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            try
            {
                await _billService.CancelBill(id, PharmacistId, request?.Reason);
                return Ok(new { message = "Bill cancelled successfully. Stock has been restored." });
            }
            catch (KeyNotFoundException ex)
            {
                // Bill does not exist.
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Bill is already cancelled — business rule conflict.
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to cancel bill. " + ex.Message });
            }
        }

        // GET: api/pharmacist/bills/{id}/invoice
        [HttpGet("{id:int}/invoice")]
        public async Task<IActionResult> Invoice(int id)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            var bill = await _billService.GetBillById(id);
            if (bill == null) return NotFound(new { message = $"Bill {id} not found." });

            return Ok(new { bill, items = await _billService.GetBillItems(id) });
        }

        // GET: api/pharmacist/bills/{id}/invoice/pdf
        [HttpGet("{id:int}/invoice/pdf")]
        public async Task<IActionResult> InvoicePdf(int id)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            var bill = await _billService.GetBillById(id);
            if (bill == null) return NotFound(new { message = $"Bill {id} not found." });

            var items    = await _billService.GetBillItems(id);
            var document = new InvoiceDocument(bill, items);
            byte[] pdf   = document.GeneratePdf();

            return File(pdf, "application/pdf", $"Invoice-{id}.pdf");
        }
    }
}
