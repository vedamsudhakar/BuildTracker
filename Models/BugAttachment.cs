using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BuildTracker.Models
{
    public class BugAttachment
    {
        public int Id { get; set; }

        public int BugId { get; set; }
        public Bug? Bug { get; set; }

        [Required]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        public string FileName { get; set; } = string.Empty;

        public DateTime UploadedDate { get; set; } = DateTime.Now;
        
        public string? UploadedByUserId { get; set; }
        public IdentityUser? UploadedByUser { get; set; }
    }
}
