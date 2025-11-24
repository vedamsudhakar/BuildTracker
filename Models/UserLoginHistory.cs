using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BuildTracker.Models
{
    public class UserLoginHistory
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public DateTime LoginTime { get; set; } = DateTime.Now;

        public string? IPAddress { get; set; }

        public string? UserAgent { get; set; }
    }
}
