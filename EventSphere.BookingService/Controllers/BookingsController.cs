using EventSphere.BookingService.Data;
using EventSphere.BookingService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EventSphere.BookingService.Controllers
{
    
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly BookingDbContext _context;

        public BookingsController(BookingDbContext context)
        {
            _context = context;
        }

        // Attendee books an event
        [HttpPost("{eventId}")]
        [Authorize(Roles = "Attendee")]
        public async Task<IActionResult> BookEvent(int eventId)
        {
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId==null)
            {
                return Unauthorized();
            }
            // Prevent duplicate booking
            var exists = await _context.Bookings
                .AnyAsync(b => b.EventId == eventId && b.UserId == userId);
            if (exists)
                return Conflict("You have already booked this event.");

            var booking = new Booking
            {
                EventId = eventId,
                UserId = userId,
            };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return Ok(booking);
        }
        // Attendee views own bookings
        [HttpGet("my")]
        [Authorize(Roles = "Attendee")]
        public async Task<IActionResult> MyBookings()
        {
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookings= await _context.Bookings
                .Where(b => b.UserId == userId)
                .ToListAsync();
            return Ok(bookings);
        }

        // Admin views all bookings
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBookings()
        {
            return Ok(await _context.Bookings.ToListAsync());
        }

    }
}
