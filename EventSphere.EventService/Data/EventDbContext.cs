using Microsoft.EntityFrameworkCore;
using EventSphere.EventService.Models;

namespace EventSphere.EventService.Data
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options)
            : base(options) { }

        public DbSet<Event> Events => Set<Event>();
    }
}
