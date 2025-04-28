namespace TrackerAPIWebApp.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }

        public int? Height { get; set; }
        public float? Weight { get; set; }

        public ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();
    }
}
