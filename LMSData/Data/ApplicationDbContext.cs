using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LMS.Core.Entities;


namespace LMS.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LMS.Core.Entities.Activity>? Activity { get; set; }
        public DbSet<LMS.Core.Entities.ActivityType>? ActivityType { get; set; }
        public DbSet<LMS.Core.Entities.Course>? Course { get; set; }
        public DbSet<LMS.Core.Entities.Document>? Document { get; set; }
        public DbSet<LMS.Core.Entities.Module>? Module { get; set; }
        public DbSet<LMS.Core.Entities.User>? User { get; set; }
    }
}