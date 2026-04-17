using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VetClinic.Model.Attributes;
using VetClinic.Model.Enums;

namespace VetClinic.Model.Entities
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Displayable("ID", 0)]
        public int Id { get; set; }

        [Searchable("Date")]
        [Displayable("Date", 1)]
        public DateTime DateTime { get; set; }

        [Searchable("Reason")]
        [Displayable("Reason", 2)]
        public string Reason { get; set; } = string.Empty;

        [Searchable("Status")]
        [Displayable("Status", 3)]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        [Displayable("Notes", 4)]
        public string Notes { get; set; } = string.Empty;

        public int DogId { get; set; }

        [ForeignKey("DogId")]
        public Dog? Dog { get; set; }

        public int VeterinarianId { get; set; }

        [ForeignKey("VeterinarianId")]
        public Veterinarian? Veterinarian { get; set; }

        public MedicalRecord? MedicalRecord { get; set; }
    }
}
