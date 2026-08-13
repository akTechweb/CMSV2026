using InfinityCoderzz_CMSV2026.Models.pharmacist;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces
{
    public interface IMedicineService
    {
        Task<IEnumerable<Medicine>> GetAllMedicines();
        Task<IEnumerable<Medicine>> SearchMedicine(string searchTerm);
        Task<Medicine?> GetMedicineById(int medicineId);
        Task AddMedicine(Medicine medicine);
        Task UpdateMedicine(Medicine medicine);
        Task DisableMedicine(int medicineId);
        Task<IEnumerable<MedicineCategory>> GetAllCategories();
        Task<IEnumerable<Manufacturer>> GetAllManufacturers();
        Task<string> GenerateNextMedicineCode();
        Task<bool> IsMedicineCodeUnique(string medicineCode);
    }
}
