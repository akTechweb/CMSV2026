using InfinityCoderzz_CMSV2026.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InfinityCoderzz_CMSV2026.Services.Pdf
{
    // Printable invoice for a reception (consultation) bill — same layout
    // language as the pharmacy invoice, so both look like they belong to
    // the same clinic.
    public class BillInvoiceDocument : IDocument
    {
        private readonly Bill _bill;

        public BillInvoiceDocument(Bill bill)
        {
            _bill = bill;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(36);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(t => t.FontSize(10).FontColor("#1e293b"));

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span("Infinity Clinic — Computer generated invoice. ").FontSize(8).FontColor("#94a3b8");
                    t.Span("Page ").FontSize(8).FontColor("#94a3b8");
                    t.CurrentPageNumber().FontSize(8).FontColor("#94a3b8");
                    t.Span(" / ").FontSize(8).FontColor("#94a3b8");
                    t.TotalPages().FontSize(8).FontColor("#94a3b8");
                });
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("Infinity Clinic").FontSize(20).Bold().FontColor("#0f766e");
                        c.Item().Text("Reception / Consultation Billing").FontSize(10).FontColor("#64748b");
                    });
                    row.ConstantItem(180).Column(c =>
                    {
                        c.Item().AlignRight().Text("INVOICE").FontSize(18).Bold();
                        c.Item().AlignRight().Text($"Bill No: {_bill.BillId}").FontSize(10);
                        c.Item().AlignRight().Text($"Date: {_bill.BillDate:dd MMM yyyy hh:mm tt}").FontSize(10);
                        c.Item().AlignRight().Text(t =>
                        {
                            t.Span("Status: ").FontSize(10).FontColor("#64748b");
                            t.Span(_bill.Status ?? "-").FontSize(10).Bold()
                             .FontColor(_bill.Status == "Paid" ? "#15803d" : "#b45309");
                        });
                    });
                });
                col.Item().PaddingTop(8).LineHorizontal(1).LineColor("#e2e8f0");
            });
        }

        private void ComposeContent(IContainer container)
        {
            var payments = _bill.Payments ?? new List<Payment>();
            decimal paid = payments
                .Where(p => p.PaymentStatus == "Completed" || p.PaymentStatus == "Paid")
                .Sum(p => p.Amount);
            decimal due = _bill.TotalAmount - paid;

            container.PaddingVertical(12).Column(col =>
            {
                col.Item().PaddingBottom(10).Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("Billed To").FontSize(9).Bold().FontColor("#64748b");
                        c.Item().Text(_bill.PatientName ?? $"Patient #{_bill.PatientId}").FontSize(12).Bold();
                        if (!string.IsNullOrWhiteSpace(_bill.PatientCode))
                            c.Item().Text($"Patient Code: {_bill.PatientCode}").FontSize(9).FontColor("#64748b");
                    });
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().AlignRight().Text("Consulted").FontSize(9).Bold().FontColor("#64748b");
                        c.Item().AlignRight().Text(_bill.DoctorName ?? "-").FontSize(12).Bold();
                        if (!string.IsNullOrWhiteSpace(_bill.AppointmentNumber))
                            c.Item().AlignRight().Text($"Appointment #: {_bill.AppointmentNumber}").FontSize(9).FontColor("#64748b");
                    });
                });

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(30);
                        columns.RelativeColumn();
                        columns.ConstantColumn(80);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderCell).Text("#");
                        header.Cell().Element(HeaderCell).Text("Description");
                        header.Cell().Element(HeaderCell).AlignRight().Text("Amount");
                    });

                    var items = _bill.Items ?? new List<BillItem>();

                    if (items.Count == 0)
                    {
                        // Bills generated straight from an appointment may not
                        // carry itemised lines — fall back to the total as a
                        // single consultation charge so the invoice isn't blank.
                        table.Cell().Element(BodyCell).Text("1");
                        table.Cell().Element(BodyCell).Text("Consultation Fee");
                        table.Cell().Element(BodyCell).AlignRight().Text($"₹{_bill.TotalAmount:N2}");
                    }
                    else
                    {
                        int idx = 1;
                        foreach (var item in items)
                        {
                            table.Cell().Element(BodyCell).Text(idx.ToString());
                            table.Cell().Element(BodyCell).Text(item.Description ?? item.ItemType ?? "Charge");
                            table.Cell().Element(BodyCell).AlignRight().Text($"₹{item.Amount:N2}");
                            idx++;
                        }
                    }
                });

                col.Item().PaddingTop(10).AlignRight().Column(c =>
                {
                    c.Item().Row(row =>
                    {
                        row.ConstantItem(120).Text("Total Amount").Bold();
                        row.ConstantItem(90).AlignRight().Text($"₹{_bill.TotalAmount:N2}").Bold().FontColor("#0f766e");
                    });

                    if (due > 0)
                    {
                        c.Item().PaddingTop(4).Row(row =>
                        {
                            row.ConstantItem(120).Text("Amount Due").Bold();
                            row.ConstantItem(90).AlignRight().Text($"₹{due:N2}").Bold().FontColor("#b45309");
                        });
                    }
                });

                if ((_bill.Status ?? "").ToLower() == "pending")
                {
                    col.Item().PaddingTop(8).AlignRight()
                       .Background("#fffbeb").Padding(6)
                       .Text("Payment Pending — please settle this invoice at the earliest.")
                       .FontSize(9).FontColor("#b45309").Italic();
                }

                if (payments.Count > 0)
                {
                    col.Item().PaddingTop(16).Text("Payments").FontSize(10).Bold();
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(80);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(HeaderCell).Text("Date");
                            header.Cell().Element(HeaderCell).Text("Method");
                            header.Cell().Element(HeaderCell).AlignRight().Text("Amount");
                            header.Cell().Element(HeaderCell).Text("Status");
                        });

                        foreach (var p in payments)
                        {
                            table.Cell().Element(BodyCell).Text(p.PaymentDate?.ToString("dd MMM yyyy") ?? "-");
                            table.Cell().Element(BodyCell).Text(p.PaymentMethod ?? "-");
                            table.Cell().Element(BodyCell).AlignRight().Text($"₹{p.Amount:N2}");
                            table.Cell().Element(BodyCell).Text(p.PaymentStatus ?? "-");
                        }
                    });
                }
            });
        }

        private static IContainer HeaderCell(IContainer container)
            => container.Background("#f0fdfa").PaddingVertical(6).PaddingHorizontal(6)
                        .BorderBottom(1).BorderColor("#99f6e4").DefaultTextStyle(t => t.Bold().FontSize(9));

        private static IContainer BodyCell(IContainer container)
            => container.PaddingVertical(5).PaddingHorizontal(6).BorderBottom(1).BorderColor("#f1f5f9");
    }
}