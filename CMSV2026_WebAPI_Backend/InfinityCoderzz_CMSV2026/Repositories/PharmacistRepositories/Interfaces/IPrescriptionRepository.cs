using InfinityCoderzz_CMSV2026.Models.pharmacist;

namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces
{
    public interface IPrescriptionRepository
    {
        Task<IEnumerable<Prescription>> GetAllPrescriptions();
        Task<Prescription?> GetPrescriptionById(int prescriptionId);
        Task<IEnumerable<PrescriptionItem>> GetPrescriptionItems(int prescriptionId);
        Task UpdatePrescriptionStatus(int prescriptionId, string status);
    }
}
