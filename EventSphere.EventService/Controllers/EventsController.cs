using EventSphere.EventService.Data;
using EventSphere.EventService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventSphere.EventService.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly EventDbContext _context;

        public EventsController(EventDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }
        [HttpPost]
        [Authorize(Roles ="Admin,Organizer")]
        public async Task<IActionResult> CreateEvent(Event evt)
        {
            _context.Events.Add(evt);
            await _context.SaveChangesAsync();
            return Ok(evt);
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="Admin,Organizer")]
        public async Task<IActionResult> UpdateEvent(int id,Event updatedEvent)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt == null)
                return NotFound();
            evt.Title = updatedEvent.Title;
            evt.Description = updatedEvent.Description;
            evt.EventDate = updatedEvent.EventDate;
            evt.Location = updatedEvent.Location;

            await _context.SaveChangesAsync();
            return Ok(evt);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt == null)
                return NotFound();
            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
