using InfinityCoderzz_CMSV2026.Models.pharmacist;

namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces
{
    public interface IMedicineStockRepository
    {
        Task<IEnumerable<MedicineStock>> GetAllMedicineStock();
        Task<MedicineStock?> GetMedicineStockById(int stockId);
        Task AddMedicineStock(MedicineStock stock);
        Task UpdateMedicineStock(MedicineStock stock);
        Task<IEnumerable<MedicineStock>> GetLowStockMedicines();
        Task<IEnumerable<Medicine>> GetAllMedicines();
        Task<IEnumerable<MedicineStock>> GetExpiringMedicines();
        Task<IEnumerable<MedicineStock>> GetExpiredMedicines();
        Task<bool> BatchExists(int medicineId, string batchNumber, int excludeStockId);
    }
}
