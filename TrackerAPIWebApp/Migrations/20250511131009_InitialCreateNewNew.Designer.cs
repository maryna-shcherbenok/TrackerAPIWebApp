﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrackerAPIWebApp;

#nullable disable

namespace TrackerAPIWebApp.Migrations
{
    [DbContext(typeof(TrackerAPIContext))]
    [Migration("20250511131009_InitialCreateNewNew")]
    partial class InitialCreateNewNew
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TrackerAPIWebApp.Models.HealthRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float?>("BodyTemperature")
                        .HasColumnType("real");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DiastolicPressure")
                        .HasColumnType("int");

                    b.Property<int?>("MoodOptionId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Pulse")
                        .HasColumnType("int");

                    b.Property<float?>("SleepHours")
                        .HasColumnType("real");

                    b.Property<int?>("Steps")
                        .HasColumnType("int");

                    b.Property<int?>("StressLevel")
                        .HasColumnType("int");

                    b.Property<int?>("SystolicPressure")
                        .HasColumnType("int");

                    b.Property<float?>("WaterIntakeLiters")
                        .HasColumnType("real");

                    b.Property<float?>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("MoodOptionId");

                    b.ToTable("HealthRecords");
                });

            modelBuilder.Entity("TrackerAPIWebApp.Models.HealthRecordMedication", b =>
                {
                    b.Property<int>("HealthRecordId")
                        .HasColumnType("int");

                    b.Property<int>("MedicationId")
                        .HasColumnType("int");

                    b.HasKey("HealthRecordId", "MedicationId");

                    b.HasIndex("MedicationId");

                    b.ToTable("HealthRecordMedications");
                });

            modelBuilder.Entity("TrackerAPIWebApp.Models.HealthRecordTag", b =>
                {
                    b.Property<int>("HealthRecordId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("HealthRecordId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("HealthRecordTags");
                });

            modelBuilder.Entity("TrackerAPIWebApp.Models.Medication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Medications");
                });

            modelBuilder.Entity("TrackerAPIWebApp.Models.MoodOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MoodOptions");
                });

            modelBuilder.Entity("TrackerAPIWebApp.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("TrackerAPIWebApp.Models.HealthRecord", b =>
                {
                    b.HasOne("TrackerAPIWebApp.Models.MoodOption", "MoodOption")
                        .WithMany("HealthRecords")
                        .HasForeignKey("MoodOptionId");

                    b.Navigation("MoodOption");
                });

            modelBuilder.Entity("TrackerAPIWebApp.Models.HealthRecordMedication", b =>
                {
                    b.HasOne("TrackerAPIWebApp.Models.HealthRecord", "HealthRecord")
                        .WithMany("Medications")
                        .HasForeignKey("HealthRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrackerAPIWebApp.Models.Medication", "Medication")
                        .WithMany("HealthRecordMedications")
                        .HasForeignKey("MedicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HealthRecord");

                    b.Navigation("Medication");
                });

            modelBuilder.Entity("TrackerAPIWebApp.Models.HealthRecordTag", b =>
                {
                    b.HasOne("TrackerAPIWebApp.Models.HealthRecord", "HealthRecord")
                        .WithMany("Tags")
                        .HasForeignKey("HealthRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrackerAPIWebApp.Models.Tag", "Tag")
                        .WithMany("HealthRecordTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HealthRecord");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("TrackerAPIWebApp.Models.HealthRecord", b =>
                {
                    b.Navigation("Medications");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("TrackerAPIWebApp.Models.Medication", b =>
                {
                    b.Navigation("HealthRecordMedications");
                });

            modelBuilder.Entity("TrackerAPIWebApp.Models.MoodOption", b =>
                {
                    b.Navigation("HealthRecords");
                });

            modelBuilder.Entity("TrackerAPIWebApp.Models.Tag", b =>
                {
                    b.Navigation("HealthRecordTags");
                });
#pragma warning restore 612, 618
        }
    }
}
