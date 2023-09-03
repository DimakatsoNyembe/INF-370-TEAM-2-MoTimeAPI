using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoTimeAPI.Models;

namespace MoTimeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimecardsController : ControllerBase
    {
        private readonly MoTimeDatabaseContext _context;

        public TimecardsController(MoTimeDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Timecards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Timecard>>> GetTimecards()
        {
            if (_context.Timecards == null)
            {
                return NotFound();
            }
            return await _context.Timecards.ToListAsync();
        }

        // GET: api/Timecards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Timecard>> GetTimecard(int id)
        {
            if (_context.Timecards == null)
            {
                return NotFound();
            }
            var timecard = await _context.Timecards.FindAsync(id);

            if (timecard == null)
            {
                return NotFound();
            }

            return timecard;
        }

        // PUT: api/Timecards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimecard(int id, Timecard timecard)
        {
            if (id != timecard.TimecardId)
            {
                return BadRequest();
            }

            _context.Entry(timecard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimecardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Timecards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Timecard>> PostTimecard(Timecard timecard)
        {
            if (_context.Timecards == null)
            {
                return Problem("Entity set 'MoTimeDatabaseContext.Timecards'  is null.");
            }
            _context.Timecards.Add(timecard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTimecard", new { id = timecard.TimecardId }, timecard);
        }

        // DELETE: api/Timecards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimecard(int id)
        {
            if (_context.Timecards == null)
            {
                return NotFound();
            }
            var timecard = await _context.Timecards.FindAsync(id);
            if (timecard == null)
            {
                return NotFound();
            }

            _context.Timecards.Remove(timecard);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TimecardExists(int id)
        {
            return (_context.Timecards?.Any(e => e.TimecardId == id)).GetValueOrDefault();
        }
    }
}


