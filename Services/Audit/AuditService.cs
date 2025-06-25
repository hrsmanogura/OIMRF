using OIMRF.Models;
using OIMRF.Properties.Data;
using OIMRF.Services.Audit;

public class AuditService : IAuditService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public void LogAudit(Guid affectedUserId, string attribute, string change, string? previous, string remarks, string? moduleOverride = null)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var controllerName = moduleOverride ?? httpContext?.GetRouteData()?.Values["controller"]?.ToString() ?? "Unknown";

        var userName = httpContext?.User?.Identity?.Name ?? "System";

        var currentUser = _context.Users.FirstOrDefault(u => u.Email == userName);
        var createdBy = currentUser != null ? $"{currentUser.FirstName} {currentUser.LastName}" : "Unknown";

        var audit = new AuditTrail
        {
            GUID = Guid.NewGuid(),
            UserId = affectedUserId,
            AttributeName = attribute,
            Change = change,
            Previous = previous,
            Remarks = remarks,
            ModuleName = controllerName,
            CreatedBy = createdBy,
            CreatedDate = DateTime.Now,
            ReportGUID = Guid.NewGuid()
        };

        _context.AuditTrail.Add(audit);
        _context.SaveChanges();
    }
}