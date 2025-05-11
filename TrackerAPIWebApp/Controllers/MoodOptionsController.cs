using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackerAPIWebApp;
using TrackerAPIWebApp.Models;

namespace TrackerAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoodOptionsController : ControllerBase
    {
        private readonly TrackerAPIContext _context;

        public MoodOptionsController(TrackerAPIContext context)
        {
            _context = context;
        }

        // GET: api/MoodOptions?name=Happy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MoodOption>>> GetMoodOptions(string? name = null)
        {
            var query = _context.MoodOptions.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.Name.Contains(name));
            }

            return await query.ToListAsync();
        }

        // GET: api/MoodOptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MoodOption>> GetMoodOption(int id)
        {
            var moodOption = await _context.MoodOptions.FindAsync(id);

            if (moodOption == null)
            {
                return NotFound();
            }

            return moodOption;
        }

        // PUT: api/MoodOptions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMoodOption(int id, MoodOption moodOption)
        {
            if (id != moodOption.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(moodOption).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoodOptionExists(id))
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

        // POST: api/MoodOptions
        [HttpPost]
        public async Task<ActionResult<MoodOption>> PostMoodOption(MoodOption moodOption)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MoodOptions.Add(moodOption);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMoodOption), new { id = moodOption.Id }, moodOption);
        }

        // DELETE: api/MoodOptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoodOption(int id)
        {
            var moodOption = await _context.MoodOptions.FindAsync(id);

            if (moodOption == null)
            {
                return NotFound();
            }

            bool isUsed = await _context.HealthRecords.AnyAsync(r => r.MoodOptionId == id);
            if (isUsed)
            {
                return BadRequest("Mood option is in use and cannot be deleted.");
            }

            _context.MoodOptions.Remove(moodOption);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MoodOptionExists(int id)
        {
            return _context.MoodOptions.Any(e => e.Id == id);
        }
    }
}
