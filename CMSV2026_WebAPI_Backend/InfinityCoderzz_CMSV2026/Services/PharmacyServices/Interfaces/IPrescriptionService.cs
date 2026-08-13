using InfinityCoderzz_CMSV2026.Models.pharmacist;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<Prescription>> GetAllPrescriptions();
        Task<Prescription?> GetPrescriptionById(int prescriptionId);
        Task<IEnumerable<PrescriptionItem>> GetPrescriptionItems(int prescriptionId);
        Task UpdatePrescriptionStatus(int prescriptionId, string status);
    }
}
