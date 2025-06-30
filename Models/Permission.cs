using OIMRF.Models;


namespace OIMRF.Models
{
    public class Permission
    {
        public Guid PermissionID { get; set; }

        public string Name { get; set; } // Example: "UserManagement"
        public string? Description { get; set; }

        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}