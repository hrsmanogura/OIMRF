using System.ComponentModel.DataAnnotations;

namespace OIMRF.ViewModels
{
    public class CreateUserViewModel
    {
        public Guid UserID { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Suffix")]
        public string? Suffix { get; set; }

        [Required]
        [Display(Name = "Contact Number")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numeric characters are allowed.")]
        public string ContactNumber { get; set; } = string.Empty;

        [Display(Name = "Role")]
        public string? Role { get; set; }

        [Display(Name = "Role ID")]
        public Guid? RoleId { get; set; }

        [Display(Name = "Is Verified")]
        public bool IsVerified { get; set; } = true;

        [Display(Name = "Is Activated")]
        public bool IsActivated { get; set; } = true;
    }
}
