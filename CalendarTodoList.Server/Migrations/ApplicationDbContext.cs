using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CalendarTodoList.Server.Models;

namespace CalendarTodoList.Server.Migrations
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Assignment> Assignments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
