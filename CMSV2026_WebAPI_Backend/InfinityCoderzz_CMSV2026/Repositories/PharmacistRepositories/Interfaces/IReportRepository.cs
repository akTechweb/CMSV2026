using InfinityCoderzz_CMSV2026.Models.pharmacist;

namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<SalesSummaryRow>> GetSalesSummary(DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<MedicineWiseSalesRow>> GetMedicineWiseSales(DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<StockStatusRow>> GetStockStatus();
        Task<IEnumerable<ExpiryReportRow>> GetExpiryReport(int days);
        Task<IEnumerable<LowStockReportRow>> GetLowStockReport();
        Task<IEnumerable<DispensingReportRow>> GetDispensingReport(DateTime? fromDate, DateTime? toDate);
    }
}
