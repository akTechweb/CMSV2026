using InfinityCoderzz_CMSV2026.Models.pharmacist;

namespace InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces
{
    public interface IAuditLogRepository
    {
        Task<IEnumerable<AuditLog>> GetAuditLogs(DateTime? fromDate, DateTime? toDate);
        Task AddAuditLog(int staffId, string action, string? remarks);
    }
}
