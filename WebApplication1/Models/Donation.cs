using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Donation
    {
        public int Id { get; set; }
        [Required]
        public string Item { get; set; }
        [Required]
        public int Quantity { get; set; }
        public DateTime DateDonated { get; set; }

        public int UserProfileId { get; set; }
        public UserProfile User { get; set; }
    }
}
