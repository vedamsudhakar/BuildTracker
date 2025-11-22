using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BuildTracker.Models
{
    public class BugHistory
    {
        public int Id { get; set; }

        public int BugId { get; set; }
        public Bug? Bug { get; set; }

        public string? ChangedByUserId { get; set; }
        public IdentityUser? ChangedByUser { get; set; }

        public DateTime ChangedDate { get; set; } = DateTime.Now;

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
