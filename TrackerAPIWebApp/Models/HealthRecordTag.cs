namespace TrackerAPIWebApp.Models
{
    public class HealthRecordTag
    {
        public int HealthRecordId { get; set; }
        public HealthRecord HealthRecord { get; set; } = null!;

        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}
