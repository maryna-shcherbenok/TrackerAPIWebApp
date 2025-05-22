using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackerAPIWebApp.Models;

namespace TrackerAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly TrackerAPIContext _context;

        public StatsController(TrackerAPIContext context)
        {
            _context = context;
        }

        // GET: api/stats/stress-last-month
        [HttpGet("stress-last-month")]
        public async Task<ActionResult<IEnumerable<object>>> GetStressLastMonth()
        {
            var monthAgo = DateTime.Today.AddDays(-30);
            var data = await _context.HealthRecords
                .Where(r => r.Date >= monthAgo && r.StressLevel != null)
                .OrderBy(r => r.Date)
                .Select(r => new
                {
                    Date = r.Date.ToString("yyyy-MM-dd"),
                    Value = r.StressLevel
                })
                .ToListAsync();

            return Ok(data);
        }

        // GET: api/stats/medication-frequency
        [HttpGet("medication-frequency")]
        public async Task<ActionResult<IEnumerable<object>>> GetMedicationFrequency()
        {
            var meds = await _context.HealthRecordMedications
                .Include(m => m.Medication) // підтягуємо назви препаратів
                .GroupBy(m => m.Medication.Name)
                .Select(g => new
                {
                    Medication = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return Ok(meds);
        }
    }
}
