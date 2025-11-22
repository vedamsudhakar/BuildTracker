using System.ComponentModel.DataAnnotations;

namespace BuildTracker.Models
{
    public class FtpServer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Server Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Host URL/IP")]
        public string Host { get; set; } = string.Empty;

        [Required]
        public int Port { get; set; } = 21;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}
