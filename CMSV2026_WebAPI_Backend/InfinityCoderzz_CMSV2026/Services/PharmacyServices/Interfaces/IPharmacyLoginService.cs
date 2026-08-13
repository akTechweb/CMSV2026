namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces
{
    public interface IPharmacyLoginService
    {
        Task<(int StaffId, string FullName)> Login(string username, string passwordHash);
    }
}
