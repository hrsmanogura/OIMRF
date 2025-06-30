namespace OIMRF.Models
{
    public class RolePermission
    {
        public Guid RolePermissionID { get; set; }
        public Guid RoleID { get; set; }
        public Guid PermissionID { get; set; }

        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }

        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}
