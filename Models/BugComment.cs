using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BuildTracker.Models
{
    public class BugComment
    {
        public int Id { get; set; }

        public int BugId { get; set; }
        public Bug? Bug { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? AuthorUserId { get; set; }
        public IdentityUser? AuthorUser { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
