using Microsoft.EntityFrameworkCore;
using VetClinic.Model.Entities;
using VetClinic.Model.Enums;

namespace VetClinic.DataLayer.Contexts
{
    public abstract class VetClinicContext : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Veterinarian> Veterinarians { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dog>()
                .HasOne(d => d.Owner)
                .WithMany(o => o.Dogs)
                .HasForeignKey(d => d.OwnerId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Dog)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DogId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Veterinarian)
                .WithMany(v => v.Appointments)
                .HasForeignKey(a => a.VeterinarianId);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Appointment)
                .WithOne(a => a.MedicalRecord)
                .HasForeignKey<MedicalRecord>(m => m.AppointmentId);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var owner1 = new Owner { Id = 1, FirstName = "Ivan", LastName = "Petrov", Phone = "+359888111222", Email = "ivan.petrov@mail.bg", Address = "Sofia, ul. Vitosha 10" };
            var owner2 = new Owner { Id = 2, FirstName = "Maria", LastName = "Ivanova", Phone = "+359888333444", Email = "maria.ivanova@mail.bg", Address = "Plovdiv, bul. Maritza 25" };
            var owner3 = new Owner { Id = 3, FirstName = "Georgi", LastName = "Dimitrov", Phone = "+359888555666", Email = "georgi.dim@mail.bg", Address = "Varna, ul. Primorski 5" };

            modelBuilder.Entity<Owner>().HasData(owner1, owner2, owner3);

            var dog1 = new Dog { Id = 1, Name = "Rex", Breed = "German Shepherd", DateOfBirth = new DateTime(2020, 3, 15), WeightKg = 34.5, ChipNumber = "BG-001-2020", OwnerId = 1 };
            var dog2 = new Dog { Id = 2, Name = "Bella", Breed = "Golden Retriever", DateOfBirth = new DateTime(2021, 7, 20), WeightKg = 28.0, ChipNumber = "BG-002-2021", OwnerId = 1 };
            var dog3 = new Dog { Id = 3, Name = "Charlie", Breed = "French Bulldog", DateOfBirth = new DateTime(2022, 1, 10), WeightKg = 12.5, ChipNumber = "BG-003-2022", OwnerId = 2 };
            var dog4 = new Dog { Id = 4, Name = "Luna", Breed = "Labrador", DateOfBirth = new DateTime(2019, 11, 5), WeightKg = 30.0, ChipNumber = "BG-004-2019", OwnerId = 3 };

            modelBuilder.Entity<Dog>().HasData(dog1, dog2, dog3, dog4);

            var vet1 = new Veterinarian { Id = 1, FirstName = "Dr. Petar", LastName = "Stoyanov", Specialization = "General", LicenseNumber = "VET-001", Phone = "+359888777888" };
            var vet2 = new Veterinarian { Id = 2, FirstName = "Dr. Elena", LastName = "Koleva", Specialization = "Surgery", LicenseNumber = "VET-002", Phone = "+359888999000" };

            modelBuilder.Entity<Veterinarian>().HasData(vet1, vet2);

            var app1 = new Appointment { Id = 1, DateTime = new DateTime(2025, 12, 1, 10, 0, 0), Reason = "Annual checkup", Status = AppointmentStatus.Completed, Notes = "Dog is healthy", DogId = 1, VeterinarianId = 1 };
            var app2 = new Appointment { Id = 2, DateTime = new DateTime(2025, 12, 5, 14, 30, 0), Reason = "Vaccination", Status = AppointmentStatus.Completed, Notes = "Rabies vaccine administered", DogId = 2, VeterinarianId = 1 };
            var app3 = new Appointment { Id = 3, DateTime = new DateTime(2026, 1, 15, 9, 0, 0), Reason = "Skin allergy", Status = AppointmentStatus.Completed, Notes = "Prescribed antihistamines", DogId = 3, VeterinarianId = 2 };
            var app4 = new Appointment { Id = 4, DateTime = new DateTime(2026, 4, 10, 11, 0, 0), Reason = "Dental cleaning", Status = AppointmentStatus.Scheduled, Notes = "", DogId = 4, VeterinarianId = 2 };
            var app5 = new Appointment { Id = 5, DateTime = new DateTime(2026, 4, 15, 16, 0, 0), Reason = "Follow-up checkup", Status = AppointmentStatus.Scheduled, Notes = "", DogId = 1, VeterinarianId = 1 };

            modelBuilder.Entity<Appointment>().HasData(app1, app2, app3, app4, app5);

            var rec1 = new MedicalRecord { Id = 1, Date = new DateTime(2025, 12, 1), Diagnosis = "Healthy", Treatment = "None required", Medications = "None", Cost = 50.00m, AppointmentId = 1 };
            var rec2 = new MedicalRecord { Id = 2, Date = new DateTime(2025, 12, 5), Diagnosis = "Vaccination due", Treatment = "Rabies vaccine", Medications = "Rabies vaccine dose", Cost = 75.00m, AppointmentId = 2 };
            var rec3 = new MedicalRecord { Id = 3, Date = new DateTime(2026, 1, 15), Diagnosis = "Allergic dermatitis", Treatment = "Topical cream + oral medication", Medications = "Cetirizine 10mg, Hydrocortisone cream", Cost = 120.00m, AppointmentId = 3 };

            modelBuilder.Entity<MedicalRecord>().HasData(rec1, rec2, rec3);
        }
    }
}
