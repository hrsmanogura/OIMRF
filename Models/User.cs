using System;
using System.ComponentModel.DataAnnotations;

namespace OIMRF.Models
{
    public class User
    {
        [Key]
        public Guid UserID { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public DateTime? LastActivityDate { get; set; }

        public Role? Role { get; set; }

        public Guid? RoleId { get; set; }

        public Guid? DesignationId { get; set; } 
        public Designation? Designation { get; set; } 

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? Suffix { get; set; }

        public string? ContactNumber { get; set; }

        public bool IsVerified { get; set; }

        public bool IsActivated { get; set; }

        public string? OTP { get; set; }

        public DateTime? OTPExpiry { get; set; }
    }
}
