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
    public class HealthRecordsController : ControllerBase
    {
        private readonly TrackerAPIContext _context;

        public HealthRecordsController(TrackerAPIContext context)
        {
            _context = context;
        }

        // GET: api/HealthRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetHealthRecords(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? tag = null)
        {
            var query = _context.HealthRecords
                .Include(r => r.MoodOption)
                .Include(r => r.Tags).ThenInclude(rt => rt.Tag)
                .Include(r => r.Medications).ThenInclude(rm => rm.Medication)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(r => r.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(r => r.Date <= endDate.Value);

            if (!string.IsNullOrEmpty(tag))
                query = query.Where(r => r.Tags.Any(rt => rt.Tag.Name == tag));

            var records = await query
                .Select(r => new
                {
                    r.Id,
                    r.Date,
                    r.Pulse,
                    r.SleepHours,
                    r.SystolicPressure,
                    r.DiastolicPressure,
                    r.BodyTemperature,
                    r.WaterIntakeLiters,
                    r.Weight,
                    r.Steps,
                    r.StressLevel,
                    r.Note,
                    Mood = r.MoodOption != null ? r.MoodOption.Name : "-",
                    Tags = r.Tags.Select(t => t.Tag.Name),
                    Medications = r.Medications.Select(m => m.Medication.Name)
                })
                .ToListAsync();

            return Ok(records);
        }


        // GET: api/HealthRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HealthRecord>> GetHealthRecord(int id)
        {
            var record = await _context.HealthRecords
                .Include(r => r.MoodOption)
                .Include(r => r.Tags).ThenInclude(rt => rt.Tag)
                .Include(r => r.Medications).ThenInclude(rm => rm.Medication)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (record == null)
            {
                return NotFound();
            }

            return record;
        }

        // PUT: api/HealthRecords/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHealthRecord(int id, HealthRecord healthRecord)
        {
            if (id != healthRecord.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(healthRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HealthRecordExists(id))
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

        [HttpPost]
        public async Task<ActionResult<HealthRecord>> PostHealthRecord(HealthRecord healthRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Перевірка: чи вже є запис на цю дату
            bool alreadyExists = await _context.HealthRecords
                .AnyAsync(r => r.Date.Date == healthRecord.Date.Date);

            if (alreadyExists)
            {
                return Conflict("На цю дату запис уже існує.");
            }

            _context.HealthRecords.Add(healthRecord);
            await _context.SaveChangesAsync();

            return Ok(new { healthRecord.Id });
        }

        // DELETE: api/HealthRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHealthRecord(int id)
        {
            var record = await _context.HealthRecords
                .Include(r => r.Tags)
                .Include(r => r.Medications)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (record == null)
            {
                return NotFound();
            }

            _context.HealthRecordTags.RemoveRange(record.Tags);
            _context.HealthRecordMedications.RemoveRange(record.Medications);
            _context.HealthRecords.Remove(record);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HealthRecordExists(int id)
        {
            return _context.HealthRecords.Any(e => e.Id == id);
        }
    }
}
