using System.ComponentModel.DataAnnotations;

namespace BuildTracker.Models
{
    public class BuildInfo
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Application")]
        public int ApplicationId { get; set; }
        public Application? Application { get; set; }

        [Required]
        [Display(Name = "Build Path")]
        public string BuildPath { get; set; } = string.Empty;

        [Display(Name = "Release Notes")]
        public string? ReleaseNotes { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string Version { get; set; } = string.Empty;

        [Display(Name = "FTP Server")]
        public int? FtpServerId { get; set; }
        public FtpServer? FtpServer { get; set; }

        public ICollection<Bug> Bugs { get; set; } = new List<Bug>();
    }
}
