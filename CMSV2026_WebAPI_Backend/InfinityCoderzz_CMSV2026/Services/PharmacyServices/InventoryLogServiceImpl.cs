using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices
{
    public class InventoryLogServiceImpl : IInventoryLogService
    {
        private readonly IInventoryLogRepository _repository;

        public InventoryLogServiceImpl(IInventoryLogRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<MedicineInventoryLog>> GetInventoryLogs()
            => _repository.GetInventoryLogs();
    }
}
