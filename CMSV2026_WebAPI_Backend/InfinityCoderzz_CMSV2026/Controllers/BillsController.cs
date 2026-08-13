using InfinityCoderzz_CMSV2026.Models;
using InfinityCoderzz_CMSV2026.Services.Pdf;
using InfinityCoderzzz_CMSV2026.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillsController : ControllerBase
    {
        private readonly IBillService _billService;
        private readonly IAppointmentService _appointmentService;

        public BillsController(
            IBillService billService,
            IAppointmentService appointmentService)
        {
            _billService = billService;
            _appointmentService = appointmentService;
        }

        // GET: api/bills
        [HttpGet]
        public ActionResult<List<Bill>> GetAll()
        {
            List<Bill> bills = _billService.GetAllBills();
            return Ok(bills);
        }

        // GET: api/bills/create-data?appointmentId=
        // Data needed to prefill a new bill for a given appointment.
        [HttpGet("create-data")]
        public IActionResult GetCreateData(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                return BadRequest(new { message = "Please select a booked appointment before billing." });
            }

            Appointment appointment = _appointmentService.GetAppointmentById(appointmentId);

            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found. Bill cannot be generated." });
            }

            if (appointment.Status == "Cancelled")
            {
                return BadRequest(new { message = "Cancelled appointment cannot be billed." });
            }

            return Ok(new Bill
            {
                PatientId = appointment.PatientId,
                AppointmentId = appointment.AppointmentId,
                PatientName = appointment.PatientName,
                PatientCode = appointment.PatientCode,
                DoctorName = appointment.DoctorName,
                AppointmentNumber = appointment.AppointmentNumber,
                ConsultationFee = appointment.ConsultationFee
            });
        }

        public class CreateBillRequest
        {
            public int PatientId { get; set; }
            public int AppointmentId { get; set; }
            public string? PaymentMethod { get; set; }
            public decimal AmountReceived { get; set; }
        }

        // POST: api/bills
        [HttpPost]
        public IActionResult Create([FromBody] CreateBillRequest request)
        {
            if (request.AmountReceived < 0)
            {
                return BadRequest(new { message = "Amount received cannot be negative." });
            }

            if (request.AmountReceived > 0)
            {
                Appointment appointment = _appointmentService.GetAppointmentById(request.AppointmentId);

                if (appointment != null && request.AmountReceived > appointment.ConsultationFee)
                {
                    return BadRequest(new { message = $"Amount received (\u20b9{request.AmountReceived}) cannot exceed the consultation fee (\u20b9{appointment.ConsultationFee})." });
                }
            }

            _billService.GenerateBill(
                request.PatientId,
                request.AppointmentId,
                out int billId,
                out decimal totalAmount,
                out string message);

            if (!string.IsNullOrWhiteSpace(message) &&
                (message.StartsWith("SUCCESS") || message.StartsWith("INFO")))
            {
                string paymentMessage = "Bill generated successfully.";

                if (request.AmountReceived > 0)
                {
                    Payment payment = new Payment
                    {
                        BillId = billId,
                        Amount = request.AmountReceived,
                        PaymentMethod = request.PaymentMethod
                    };

                    _billService.ProcessPayment(payment, out string payMessage);

                    paymentMessage = !string.IsNullOrWhiteSpace(payMessage) && payMessage.StartsWith("SUCCESS")
                        ? "Bill generated and payment received successfully."
                        : $"Bill generated (#{billId}), but the payment could not be recorded: {payMessage}. You can retry from the bill's Receive Payment action.";
                }

                return CreatedAtAction(nameof(GetById), new { id = billId }, new
                {
                    billId,
                    totalAmount,
                    message = paymentMessage
                });
            }

            return BadRequest(new { message });
        }

        // GET: api/bills/5
        [HttpGet("{id}")]
        public ActionResult<Bill> GetById(int id)
        {
            Bill bill = _billService.GetBillById(id);

            if (bill == null)
                return NotFound();

            return Ok(bill);
        }

        // GET: api/bills/5/pdf
        [HttpGet("{id}/pdf")]
        public IActionResult Pdf(int id)
        {
            Bill bill = _billService.GetBillById(id);

            if (bill == null)
                return NotFound();

            var document = new BillInvoiceDocument(bill);
            byte[] pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Invoice-{id}.pdf");
        }

        public class ReceivePaymentRequest
        {
            public string? PaymentMethod { get; set; }
        }

        // POST: api/bills/5/payments
        [HttpPost("{id}/payments")]
        public IActionResult ReceivePayment(int id, [FromBody] ReceivePaymentRequest request)
        {
            Bill bill = _billService.GetBillById(id);

            if (bill == null)
                return NotFound();

            if (bill.Status == "Paid")
            {
                return Ok(new { message = "Bill is already fully paid." });
            }

            if (string.IsNullOrWhiteSpace(request.PaymentMethod))
            {
                return BadRequest(new { message = "Please select a payment method." });
            }

            decimal paidAmount = bill.Payments?
                .Where(p => p.PaymentStatus == "Completed")
                .Sum(p => p.Amount) ?? 0;

            decimal remainingAmount = bill.TotalAmount - paidAmount;

            if (remainingAmount <= 0)
            {
                return Ok(new { message = "Bill is already fully paid." });
            }

            Payment payment = new Payment
            {
                BillId = id,
                Amount = remainingAmount,
                PaymentMethod = request.PaymentMethod,
                TransactionReference = null
            };

            _billService.ProcessPayment(payment, out string message);

            if (!string.IsNullOrWhiteSpace(message) &&
                message.StartsWith("SUCCESS"))
            {
                return Ok(new { message = "Payment received successfully.", amountPaid = remainingAmount });
            }

            return BadRequest(new { message });
        }
    }
}