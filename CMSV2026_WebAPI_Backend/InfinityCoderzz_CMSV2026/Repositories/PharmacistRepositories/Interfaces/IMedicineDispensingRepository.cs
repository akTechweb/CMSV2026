using InfinityCoderzz_CMSV2026.Models.pharmacist;

namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces
{
    public interface IMedicineDispensingRepository
    {
        Task<IEnumerable<Prescription>> GetDispensablePrescriptions();
        Task<DispenseBillResult> DispenseAndBill(int prescriptionId, int staffId, string? remarks);
        Task<IEnumerable<DispensingHistoryViewModel>> GetDispensingHistory();
        Task<IEnumerable<MedicineDispensingItem>> GetDispensingItems(int dispenseId);

        Task<StockCheckResult> CheckStock(int prescriptionId);
    }
}
