using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class VolunteerTask
    {
        public int Id { get; set; }
        [Required]
        public string TaskName { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string Status { get; set; } = "Open"; // Open, Assigned, Completed

        public int? UserProfileId { get; set; }  // assigned volunteer (nullable)
        public UserProfile User { get; set; }
    }
}
