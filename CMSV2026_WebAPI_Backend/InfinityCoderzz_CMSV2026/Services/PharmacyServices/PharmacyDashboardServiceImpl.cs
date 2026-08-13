using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices
{
    public class PharmacyDashboardServiceImpl : IPharmacyDashboardService
    {
        private readonly IPharmacyDashboardRepository _dashboardRepository;

        public PharmacyDashboardServiceImpl(IPharmacyDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public Task<PharmacyDashboard> GetDashboardData()
            => _dashboardRepository.GetDashboardData();
    }
}
