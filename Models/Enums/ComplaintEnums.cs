using System.ComponentModel.DataAnnotations;

namespace OIMRF.Models.Enums
{
    public enum Platform
    {
        [Display(Name = "Facebook")]
        Facebook = 1,
        [Display(Name = "Twitter")]
        Twitter = 2,
        [Display(Name = "Instagram")]
        Instagram = 3,
        [Display(Name = "TikTok")]
        TikTok = 4,
        [Display(Name = "YouTube")]
        YouTube = 5
    }


    public enum Severity
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
}
