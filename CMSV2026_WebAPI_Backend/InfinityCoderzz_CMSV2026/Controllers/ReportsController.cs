using System.Text;
using Microsoft.AspNetCore.Mvc;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzzz_CMSV2026.Controllers
{
    [ApiController]
    [Route("api/pharmacist/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;
        public ReportsController(IReportService service) => _service = service;

        private int PharmacistId => HttpContext.Session.GetInt32("PharmacistId") ?? 0;

        // GET: api/pharmacist/reports?report=sales&fromDate=&toDate=&days=30
        // report: sales | medicinewise | stock | expiry | lowstock | dispensing
        [HttpGet]
        public async Task<IActionResult> Index(string report = "sales", DateTime? fromDate = null,
            DateTime? toDate = null, int days = 30)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            object data = report switch
            {
                "medicinewise" => await _service.GetMedicineWiseSales(fromDate, toDate),
                "stock"        => await _service.GetStockStatus(),
                "expiry"       => await _service.GetExpiryReport(days),
                "lowstock"     => await _service.GetLowStockReport(),
                "dispensing"   => await _service.GetDispensingReport(fromDate, toDate),
                _              => await _service.GetSalesSummary(fromDate, toDate)
            };

            string resolvedReport = report switch
            {
                "medicinewise" or "stock" or "expiry" or "lowstock" or "dispensing" => report,
                _ => "sales"
            };

            return Ok(new { report = resolvedReport, fromDate, toDate, days, data });
        }

        // GET: api/pharmacist/reports/export?report=sales&fromDate=&toDate=&days=30
        [HttpGet("export")]
        public async Task<IActionResult> ExportCsv(string report = "sales", DateTime? fromDate = null,
            DateTime? toDate = null, int days = 30)
        {
            if (PharmacistId == 0) return Unauthorized(new { message = "Not logged in." });

            string csv;
            string fileName;

            switch (report)
            {
                case "medicinewise":
                    csv = ToCsv(new[] { "MedicineId", "MedicineName", "QuantitySold", "TotalAmount" },
                        (await _service.GetMedicineWiseSales(fromDate, toDate))
                            .Select(r => new[] { r.MedicineId.ToString(), r.MedicineName, r.QuantitySold.ToString(), r.TotalAmount.ToString("0.00") }));
                    fileName = "medicine-wise-sales.csv";
                    break;
                case "stock":
                    csv = ToCsv(new[] { "MedicineCode", "MedicineName", "ReorderLevel", "TotalQuantity", "StockStatus" },
                        (await _service.GetStockStatus())
                            .Select(r => new[] { r.MedicineCode, r.MedicineName, r.ReorderLevel.ToString(), r.TotalQuantity.ToString(), r.StockStatus }));
                    fileName = "stock-status.csv";
                    break;
                case "expiry":
                    csv = ToCsv(new[] { "MedicineCode", "MedicineName", "BatchNumber", "Quantity", "ExpiryDate", "DaysRemaining", "ExpiryStatus" },
                        (await _service.GetExpiryReport(days))
                            .Select(r => new[] { r.MedicineCode, r.MedicineName, r.BatchNumber, r.Quantity.ToString(), r.ExpiryDate.ToString("yyyy-MM-dd"), r.DaysRemaining.ToString(), r.ExpiryStatus }));
                    fileName = "expiry-report.csv";
                    break;
                case "lowstock":
                    csv = ToCsv(new[] { "MedicineCode", "MedicineName", "ReorderLevel", "TotalQuantity" },
                        (await _service.GetLowStockReport())
                            .Select(r => new[] { r.MedicineCode, r.MedicineName, r.ReorderLevel.ToString(), r.TotalQuantity.ToString() }));
                    fileName = "low-stock-report.csv";
                    break;
                case "dispensing":
                    csv = ToCsv(new[] { "DispenseId", "PrescriptionId", "PatientName", "PharmacistName", "DispenseDate", "TotalItems", "TotalAmount" },
                        (await _service.GetDispensingReport(fromDate, toDate))
                            .Select(r => new[] { r.DispenseId.ToString(), r.PrescriptionId.ToString(), r.PatientName, r.PharmacistName, r.DispenseDate.ToString("yyyy-MM-dd HH:mm"), r.TotalItems.ToString(), r.TotalAmount.ToString("0.00") }));
                    fileName = "dispensing-report.csv";
                    break;
                default:
                    csv = ToCsv(new[] { "SaleDate", "BillCount", "ItemsSold", "TotalAmount" },
                        (await _service.GetSalesSummary(fromDate, toDate))
                            .Select(r => new[] { r.SaleDate.ToString("yyyy-MM-dd"), r.BillCount.ToString(), r.ItemsSold.ToString(), r.TotalAmount.ToString("0.00") }));
                    fileName = "sales-summary.csv";
                    break;
            }

            return File(Encoding.UTF8.GetBytes(csv), "text/csv", fileName);
        }

        private static string ToCsv(string[] headers, IEnumerable<string?[]> rows)
        {
            StringBuilder sb = new();
            sb.AppendLine(string.Join(",", headers.Select(Escape)));
            foreach (var row in rows)
                sb.AppendLine(string.Join(",", row.Select(Escape)));
            return sb.ToString();
        }

        private static string Escape(string? value)
        {
            value ??= string.Empty;
            if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            return value;
        }
    }
}
