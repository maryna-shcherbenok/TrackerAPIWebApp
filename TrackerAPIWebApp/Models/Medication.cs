namespace TrackerAPIWebApp.Models
{
    public class Medication
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<HealthRecordMedication> HealthRecordMedications { get; set; } = new List<HealthRecordMedication>();
    }
}
