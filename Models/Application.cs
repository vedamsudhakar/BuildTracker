using System.ComponentModel.DataAnnotations;

namespace BuildTracker.Models
{
    public class Application
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Application Name")]
        public string Name { get; set; } = string.Empty;
    }
}
