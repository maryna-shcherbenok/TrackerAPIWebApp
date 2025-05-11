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

        // GET: api/stats/average
        [HttpGet("average")]
        public async Task<ActionResult<object>> GetAverageStats()
        {
            var records = await _context.HealthRecords.ToListAsync();

            if (records.Count == 0)
                return NotFound("No records available.");

            var result = new
            {
                AvgPulse = records.Where(r => r.Pulse.HasValue).Average(r => (double?)r.Pulse),
                AvgSleep = records.Where(r => r.SleepHours.HasValue).Average(r => (double?)r.SleepHours),
                AvgWater = records.Where(r => r.WaterIntakeLiters.HasValue).Average(r => (double?)r.WaterIntakeLiters),
                AvgSteps = records.Where(r => r.Steps.HasValue).Average(r => (double?)r.Steps),
                AvgStress = records.Where(r => r.StressLevel.HasValue).Average(r => (double?)r.StressLevel),
                AvgTemperature = records.Where(r => r.BodyTemperature.HasValue).Average(r => (double?)r.BodyTemperature),
                AvgWeight = records.Where(r => r.Weight.HasValue).Average(r => (double?)r.Weight)
            };

            return Ok(result);
        }

        // GET: api/stats/weekly-summary
        [HttpGet("weekly-summary")]
        public async Task<ActionResult<object>> GetWeeklySummary()
        {
            var weekAgo = DateTime.Today.AddDays(-7);
            var recent = await _context.HealthRecords
                .Where(r => r.Date >= weekAgo)
                .ToListAsync();

            if (recent.Count == 0)
                return NotFound("No data for the past week.");

            var result = new
            {
                DaysCount = recent.Select(r => r.Date.Date).Distinct().Count(),
                RecordsCount = recent.Count,
                AvgPulse = recent.Where(r => r.Pulse.HasValue).Average(r => (double?)r.Pulse),
                TotalSteps = recent.Where(r => r.Steps.HasValue).Sum(r => r.Steps),
                AvgSleep = recent.Where(r => r.SleepHours.HasValue).Average(r => (double?)r.SleepHours),
                AvgWater = recent.Where(r => r.WaterIntakeLiters.HasValue).Average(r => (double?)r.WaterIntakeLiters),
                AvgWeight = recent.Where(r => r.Weight.HasValue).Average(r => (double?)r.Weight)
            };

            return Ok(result);
        }

        // GET: api/stats/chart-data?param=weight
        [HttpGet("chart-data")]
        public async Task<ActionResult<IEnumerable<object>>> GetChartData([FromQuery] string param = "pulse")
        {
            var records = await _context.HealthRecords
                .OrderBy(r => r.Date)
                .ToListAsync();

            var chartData = records
                .Select(r => new {
                    Date = r.Date.ToString("yyyy-MM-dd"),
                    Value = param.ToLower() switch
                    {
                        "pulse" => (double?)r.Pulse,
                        "sleep" => (double?)r.SleepHours,
                        "weight" => (double?)r.Weight,
                        "temperature" => (double?)r.BodyTemperature,
                        "water" => (double?)r.WaterIntakeLiters,
                        "steps" => (double?)r.Steps,
                        "stress" => (double?)r.StressLevel,
                        _ => null
                    }
                })
                .Where(p => p.Value != null)
                .ToList();

            if (chartData.Count == 0)
                return NotFound($"No data found for parameter: {param}");

            return Ok(chartData);
        }

        // GET: api/stats/bmi-history
        [HttpGet("bmi-history")]
        public ActionResult GetBmiHistory()
        {
            return BadRequest("Обчислення ІМТ наразі недоступне: поле Height відсутнє.");
        }
    }
}
