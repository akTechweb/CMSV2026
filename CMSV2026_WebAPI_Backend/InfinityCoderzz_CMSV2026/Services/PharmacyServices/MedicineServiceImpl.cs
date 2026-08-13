using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices
{
    public class MedicineServiceImpl : IMedicineService
    {
        private readonly IMedicineRepository _medicineRepository;

        public MedicineServiceImpl(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }

        public Task<IEnumerable<Medicine>>         GetAllMedicines()                   => _medicineRepository.GetAllMedicines();
        public Task<IEnumerable<Medicine>>         SearchMedicine(string searchTerm)   => _medicineRepository.SearchMedicine(searchTerm);
        public Task<Medicine?>                     GetMedicineById(int medicineId)     => _medicineRepository.GetMedicineById(medicineId);
        public Task                                AddMedicine(Medicine medicine)      => _medicineRepository.AddMedicine(medicine);
        public Task                                UpdateMedicine(Medicine medicine)   => _medicineRepository.UpdateMedicine(medicine);
        public Task                                DisableMedicine(int medicineId)     => _medicineRepository.DisableMedicine(medicineId);
        public Task<IEnumerable<MedicineCategory>> GetAllCategories()                  => _medicineRepository.GetAllCategories();
        public Task<IEnumerable<Manufacturer>>     GetAllManufacturers()               => _medicineRepository.GetAllManufacturers();

        #region Medicine Code Generation

        public async Task<string> GenerateNextMedicineCode()
        {
            const string prefix = "MED-";
            int max = 0;

            foreach (var medicine in await _medicineRepository.GetAllMedicines())
            {
                var code = medicine.MedicineCode;
                if (string.IsNullOrWhiteSpace(code)) continue;
                if (!code.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) continue;
                if (int.TryParse(code[prefix.Length..], out int number) && number > max)
                    max = number;
            }

            return prefix + (max + 1).ToString("D6");
        }

        public async Task<bool> IsMedicineCodeUnique(string medicineCode)
        {
            if (string.IsNullOrWhiteSpace(medicineCode)) return false;

            return !(await _medicineRepository.GetAllMedicines())
                .Any(m => string.Equals(m.MedicineCode?.Trim(), medicineCode.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
