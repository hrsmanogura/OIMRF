namespace OIMRF.Services.Audit
{
    public interface IAuditService
    {
        void LogAudit(Guid affectedUserId, string attribute, string change, string? previous, string remarks, string? moduleOverride = null);
    }
}
