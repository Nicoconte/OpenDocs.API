using Microsoft.EntityFrameworkCore;
using OpenDocs.API.Models;

namespace OpenDocs.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Settings> Settings { get; set; }
        public DbSet<Applications> Applications { get; set; }
        public DbSet<Administrators> Administrators { get; set; }
        public DbSet<Models.Environments> Environments { get; set; }
    }
}
