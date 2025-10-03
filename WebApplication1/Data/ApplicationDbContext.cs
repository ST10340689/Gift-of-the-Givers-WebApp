using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<DisasterReport> DisasterReports { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<VolunteerTask> VolunteerTasks { get; set; }
    }
}
