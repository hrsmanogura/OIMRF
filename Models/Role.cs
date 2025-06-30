namespace OIMRF.Models
{
    public class Role
    {
        public Guid RoleID { get; set; }
        public string RoleName { get; set; } = string.Empty;

        public string? Description { get; set; }
        public ICollection<User> Users { get; set; } // navigation

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
