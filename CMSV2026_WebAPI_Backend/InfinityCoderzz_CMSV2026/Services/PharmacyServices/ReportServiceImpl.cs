using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices
{
    public class ReportServiceImpl : IReportService
    {
        private readonly IReportRepository _repository;

        public ReportServiceImpl(IReportRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<SalesSummaryRow>>      GetSalesSummary(DateTime? from, DateTime? to)    => _repository.GetSalesSummary(from, to);
        public Task<IEnumerable<MedicineWiseSalesRow>> GetMedicineWiseSales(DateTime? from, DateTime? to) => _repository.GetMedicineWiseSales(from, to);
        public Task<IEnumerable<StockStatusRow>>       GetStockStatus()                                  => _repository.GetStockStatus();
        public Task<IEnumerable<ExpiryReportRow>>      GetExpiryReport(int days)                         => _repository.GetExpiryReport(days);
        public Task<IEnumerable<LowStockReportRow>>    GetLowStockReport()                               => _repository.GetLowStockReport();
        public Task<IEnumerable<DispensingReportRow>>  GetDispensingReport(DateTime? from, DateTime? to) => _repository.GetDispensingReport(from, to);
    }
}
