namespace TrackerAPIWebApp.Models
{
    public class HealthRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int? Pulse { get; set; }
        public int? SystolicPressure { get; set; }
        public int? DiastolicPressure { get; set; }

        public float? SleepHours { get; set; }
        public float? BodyTemperature { get; set; }
        public float? WaterIntakeLiters { get; set; }

        public int? StressLevel { get; set; }
        public int? Steps { get; set; }

        public float? Weight { get; set; }       

        public string? Note { get; set; }

        public int? MoodOptionId { get; set; }
        public MoodOption? MoodOption { get; set; }

        public ICollection<HealthRecordMedication> Medications { get; set; } = new List<HealthRecordMedication>();
        public ICollection<HealthRecordTag> Tags { get; set; } = new List<HealthRecordTag>();
    }
}
