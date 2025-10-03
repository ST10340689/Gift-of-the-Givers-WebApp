using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        [Required] public string FullName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required] public string PasswordHash { get; set; } // hashed password
        public string Role { get; set; } = "User";

        public ICollection<DisasterReport> Reports { get; set; }
        public ICollection<Donation> Donations { get; set; }
        public ICollection<VolunteerTask> VolunteerTasks { get; set; }
    }
}
