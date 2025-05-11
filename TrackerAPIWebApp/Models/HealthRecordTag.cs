namespace TrackerAPIWebApp.Models
{
    public class HealthRecordTag
    {
        public int HealthRecordId { get; set; }
        public HealthRecord? HealthRecord { get; set; }

        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
