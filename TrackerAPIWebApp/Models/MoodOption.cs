namespace TrackerAPIWebApp.Models
{
    public class MoodOption
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();
    }
}
