using InfinityCoderzz_CMSV2026.Models.pharmacist;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces
{
    public interface IAuditLogService
    {
        Task<IEnumerable<AuditLog>> GetAuditLogs(DateTime? fromDate, DateTime? toDate);
        Task AddAuditLog(int staffId, string action, string? remarks);
    }
}
