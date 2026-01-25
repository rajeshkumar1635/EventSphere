using Microsoft.EntityFrameworkCore;
using EventSphere.BookingService.Models;

namespace EventSphere.BookingService.Data
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options)
            : base(options) { }

        public DbSet<Booking> Bookings => Set<Booking>();
    }
}
