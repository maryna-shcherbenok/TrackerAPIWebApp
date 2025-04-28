﻿namespace TrackerAPIWebApp.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<HealthRecordTag> HealthRecordTags { get; set; } = new List<HealthRecordTag>();
    }
}
