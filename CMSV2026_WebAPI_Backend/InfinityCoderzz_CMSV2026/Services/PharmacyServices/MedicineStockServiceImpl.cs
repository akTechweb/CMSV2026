using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices
{
    public class MedicineStockServiceImpl : IMedicineStockService
    {
        private readonly IMedicineStockRepository _medicineStockRepository;

        public MedicineStockServiceImpl(IMedicineStockRepository medicineStockRepository)
        {
            _medicineStockRepository = medicineStockRepository;
        }

        public Task<IEnumerable<MedicineStock>> GetAllMedicineStock()                                                   => _medicineStockRepository.GetAllMedicineStock();
        public Task<MedicineStock?>              GetMedicineStockById(int stockId)                                       => _medicineStockRepository.GetMedicineStockById(stockId);
        public Task                              AddMedicineStock(MedicineStock stock)                                   => _medicineStockRepository.AddMedicineStock(stock);
        public Task                              UpdateMedicineStock(MedicineStock stock)                                => _medicineStockRepository.UpdateMedicineStock(stock);
        public Task<IEnumerable<MedicineStock>> GetLowStockMedicines()                                                  => _medicineStockRepository.GetLowStockMedicines();
        public Task<IEnumerable<Medicine>>      GetAllMedicines()                                                       => _medicineStockRepository.GetAllMedicines();
        public Task<IEnumerable<MedicineStock>> GetExpiringMedicines()                                                  => _medicineStockRepository.GetExpiringMedicines();
        public Task<IEnumerable<MedicineStock>> GetExpiredMedicines()                                                   => _medicineStockRepository.GetExpiredMedicines();
        public Task<bool>                        BatchExists(int medicineId, string batchNumber, int excludeStockId)    => _medicineStockRepository.BatchExists(medicineId, batchNumber, excludeStockId);
    }
}
