using System;
using System.ComponentModel.DataAnnotations;
using OIMRF.Models.Enums;

namespace OIMRF.Models
{
    public class Complaints
    {
        public Guid ComplaintGUID { get; set; }
        [Key]
        public string ComplaintID { get; set; } = String.Empty;
        public string? Location { get; set; }
        public DateTime? OriginalPostDate { get; set; }
        public string? OriginalPost { get; set; }
        public Platform? Platform { get; set; }
        public string? AccountProfileName { get; set; }
        public DateTime? DateMonitored { get; set; }
        public string? PIDInCharge { get; set; }
        public int? NumberOfFollowers { get; set; }
        public int? NumberOfReactions { get; set; }
        public int? NumberOfShares { get; set; }
        public int? NumberOfViews { get; set; }
        public string? Issues { get; set; }
        public string? BureauAgencyInvolved { get; set; }
        public string? EnvironmentalLawsConcerned { get; set; }
        public Severity? SeverityOfComplaints { get; set; }
        public string? Remarks { get; set; }
        public string? DigitalLink { get; set; }
        public string? OfficerInCharge { get; set; }
        public DateTime? DateIssued { get; set; }
        public string? SpecificActionTaken { get; set; }
        public string? ResolutionTime { get; set; }
        public string? ResolutionStatus { get; set; }
        public string? EscalationDetails { get; set; }
        public bool? RecurringIssue { get; set; }
        public string? RootCauseAnalysis { get; set; }
        public string? CustomerSentiment { get; set; }
        public string? KeyWords { get; set; }
        public string? LessonLearned { get; set; }
        
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
