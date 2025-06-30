using System.ComponentModel.DataAnnotations;

namespace OIMRF.ViewModels
{
    public class RoleViewModel
    {
        public Guid? RoleID { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        public bool IsActive { get; set; } = true;
    }

}
