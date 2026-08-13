using InfinityCoderzz_CMSV2026.Models.pharmacist;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces
{
    public interface IPharmacyDashboardService
    {
        Task<PharmacyDashboard> GetDashboardData();
    }
}
