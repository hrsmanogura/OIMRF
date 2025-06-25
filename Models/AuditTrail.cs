using System;
using System.ComponentModel.DataAnnotations;

namespace OIMRF.Models
{
    public class AuditTrail
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public Guid GUID { get; set; }

        public Guid? UserId { get; set; }

        public string? Change { get; set; }

        public string? Previous { get; set; }

        [StringLength(50)]
        public string? ModuleName { get; set; }

        [StringLength(50)]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? ReportGUID { get; set; }

        [StringLength(256)]
        public string? AttributeName { get; set; }

        [StringLength(256)]
        public string? Remarks { get; set; }
    }
}
