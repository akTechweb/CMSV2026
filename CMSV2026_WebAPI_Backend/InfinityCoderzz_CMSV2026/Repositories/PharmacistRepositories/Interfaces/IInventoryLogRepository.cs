using InfinityCoderzz_CMSV2026.Models.pharmacist;

namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces
{
    public interface IInventoryLogRepository
    {
        Task<IEnumerable<MedicineInventoryLog>> GetInventoryLogs();
    }
}
