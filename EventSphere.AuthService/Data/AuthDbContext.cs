using Microsoft.EntityFrameworkCore;
using EventSphere.AuthService.Models;

namespace EventSphere.AuthService.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
