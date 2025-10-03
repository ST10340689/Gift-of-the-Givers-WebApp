using System.Runtime.CompilerServices;
using WebApplication1.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class DisasterReport
    {

        // Getters and Setters
        public int Id { get; set; }

        [Required]
        public string DisasterType { get; set; }
        [Required]
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime ReportDate { get; set; }

        public int UserProfileId { get; set; }
        public UserProfile User { get; set; }
    }
}
