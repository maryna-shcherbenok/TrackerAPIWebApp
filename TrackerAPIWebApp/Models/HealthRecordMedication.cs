namespace TrackerAPIWebApp.Models
{
    public class HealthRecordMedication
    {
        public int HealthRecordId { get; set; }
        public HealthRecord HealthRecord { get; set; } = null!;

        public int MedicationId { get; set; }
        public Medication Medication { get; set; } = null!;
    }
}
