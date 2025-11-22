using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BuildTracker.Models
{
    public enum BugType
    {
        UI,
        Functional,
        Performance,
        Security,
        Compatibility,
        Other
    }

    public enum BugSeverity
    {
        Critical,
        High,
        Medium,
        Low
    }

    public enum BugStatus
    {
        Open,
        InProgress,
        Fixed,
        Reopened,
        Closed
    }

    public class Bug
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Application")]
        public int ApplicationId { get; set; }
        public Application? Application { get; set; }

        [Display(Name = "Build")]
        public int? BuildId { get; set; }
        public BuildInfo? Build { get; set; }

        [Required]
        public BugType Type { get; set; }

        [Required]
        public BugSeverity Severity { get; set; }

        [Required]
        public BugStatus Status { get; set; } = BugStatus.Open;

        [Display(Name = "Environment Details")]
        public string? EnvironmentDetails { get; set; }

        [Display(Name = "Steps to Reproduce")]
        public string? StepsToReproduce { get; set; }

        [Display(Name = "Expected Result")]
        public string? ExpectedResult { get; set; }

        [Display(Name = "Actual Result")]
        public string? ActualResult { get; set; }

        [Display(Name = "Assigned To")]
        public string? AssignedToUserId { get; set; }
        public IdentityUser? AssignedToUser { get; set; }

        public string? CreatedByUserId { get; set; }
        public IdentityUser? CreatedByUser { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? UpdatedByUserId { get; set; }
        public IdentityUser? UpdatedByUser { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<BugAttachment> Attachments { get; set; } = new List<BugAttachment>();
        public ICollection<BugComment> Comments { get; set; } = new List<BugComment>();
        public ICollection<BugHistory> History { get; set; } = new List<BugHistory>();
    }
}
