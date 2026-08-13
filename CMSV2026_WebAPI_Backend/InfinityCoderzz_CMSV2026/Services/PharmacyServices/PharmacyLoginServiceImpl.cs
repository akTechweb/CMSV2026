using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices
{
    public class PharmacyLoginServiceImpl : IPharmacyLoginService
    {
        private readonly IPharmacyLoginRepository _repository;

        public PharmacyLoginServiceImpl(IPharmacyLoginRepository repository)
        {
            _repository = repository;
        }

        public Task<(int StaffId, string FullName)> Login(string username, string passwordHash)
            => _repository.Login(username, passwordHash);
    }
}
