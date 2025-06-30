namespace OIMRF.ViewModels
{
    public class RoleWithPermissionsViewModel
    {
        public Guid RoleID { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }

}
