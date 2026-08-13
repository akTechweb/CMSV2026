using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices
{
    public class MedicineDispensingServiceImpl : IMedicineDispensingService
    {
        private readonly IMedicineDispensingRepository _repository;

        public MedicineDispensingServiceImpl(IMedicineDispensingRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Prescription>> GetDispensablePrescriptions() => _repository.GetDispensablePrescriptions();
        public Task<DispenseBillResult> DispenseAndBill(int prescriptionId, int staffId, string? remarks) => _repository.DispenseAndBill(prescriptionId, staffId, remarks);
        public Task<IEnumerable<DispensingHistoryViewModel>> GetDispensingHistory() => _repository.GetDispensingHistory();
        public Task<IEnumerable<MedicineDispensingItem>> GetDispensingItems(int dispenseId) => _repository.GetDispensingItems(dispenseId);
        public Task<StockCheckResult> CheckStock(int prescriptionId) => _repository.CheckStock(prescriptionId);
    }
}

