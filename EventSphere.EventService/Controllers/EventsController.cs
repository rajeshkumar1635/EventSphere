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
    }
}
