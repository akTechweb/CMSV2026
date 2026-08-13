using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices
{
    public class PrescriptionServiceImpl : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;

        public PrescriptionServiceImpl(IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        public Task<IEnumerable<Prescription>>     GetAllPrescriptions()                                   => _prescriptionRepository.GetAllPrescriptions();
        public Task<Prescription?>                  GetPrescriptionById(int prescriptionId)                 => _prescriptionRepository.GetPrescriptionById(prescriptionId);
        public Task<IEnumerable<PrescriptionItem>> GetPrescriptionItems(int prescriptionId)                 => _prescriptionRepository.GetPrescriptionItems(prescriptionId);
        public Task                                 UpdatePrescriptionStatus(int prescriptionId, string s)  => _prescriptionRepository.UpdatePrescriptionStatus(prescriptionId, s);
    }
}
