namespace OIMRF.Models
{
    public class Role
    {
        public Guid RoleID { get; set; }
        public string RoleName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
