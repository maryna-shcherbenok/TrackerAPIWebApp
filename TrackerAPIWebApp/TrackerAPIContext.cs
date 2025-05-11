using Microsoft.EntityFrameworkCore;
using TrackerAPIWebApp.Models;

namespace TrackerAPIWebApp
{
    public class TrackerAPIContext : DbContext
    {
        public TrackerAPIContext(DbContextOptions<TrackerAPIContext> options)
            : base(options)
        {
        }

        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<MoodOption> MoodOptions { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<HealthRecordMedication> HealthRecordMedications { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<HealthRecordTag> HealthRecordTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HealthRecordMedication>()
                .HasKey(hrm => new { hrm.HealthRecordId, hrm.MedicationId });

            modelBuilder.Entity<HealthRecordMedication>()
                .HasOne(hrm => hrm.HealthRecord)
                .WithMany(hr => hr.Medications)
                .HasForeignKey(hrm => hrm.HealthRecordId);

            modelBuilder.Entity<HealthRecordMedication>()
                .HasOne(hrm => hrm.Medication)
                .WithMany(m => m.HealthRecordMedications)
                .HasForeignKey(hrm => hrm.MedicationId);

            modelBuilder.Entity<HealthRecordTag>()
                .HasKey(hrt => new { hrt.HealthRecordId, hrt.TagId });

            modelBuilder.Entity<HealthRecordTag>()
                .HasOne(hrt => hrt.HealthRecord)
                .WithMany(hr => hr.Tags)
                .HasForeignKey(hrt => hrt.HealthRecordId);

            modelBuilder.Entity<HealthRecordTag>()
                .HasOne(hrt => hrt.Tag)
                .WithMany(t => t.HealthRecordTags)
                .HasForeignKey(hrt => hrt.TagId);
        }
    }
}
