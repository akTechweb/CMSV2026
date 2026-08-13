using InfinityCoderzz_CMSV2026.Models.pharmacist;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces
{
    public interface IInventoryLogService
    {
        Task<IEnumerable<MedicineInventoryLog>> GetInventoryLogs();
    }
}
