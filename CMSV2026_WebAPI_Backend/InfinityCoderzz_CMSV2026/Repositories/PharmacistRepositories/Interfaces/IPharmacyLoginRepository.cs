namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces
{
    public interface IPharmacyLoginRepository
    {
        Task<(int StaffId, string FullName)> Login(string username, string passwordHash);
    }
}
