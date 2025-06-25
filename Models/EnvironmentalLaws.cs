using System.ComponentModel.DataAnnotations;

namespace OIMRF.Models
{
    public class EnvironmentalLaws
    {
        [Key]
        public int LawID { get; set; }
        public string? LawCode { get; set; }
        public string? LawTitle { get; set; }
        public string? Description { get; set; }
    }
}
