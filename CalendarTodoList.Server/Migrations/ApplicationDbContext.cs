using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestV2.Server.Models;

namespace TestV2.Server.Migrations
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Assignment> Assignments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>()
                .Property(e => e.Id)
                .HasDefaultValueSql("newsequentialid()");

            base.OnModelCreating(modelBuilder);
        }
    }
}
