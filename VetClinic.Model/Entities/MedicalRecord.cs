using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VetClinic.Model.Attributes;

namespace VetClinic.Model.Entities
{
    public class MedicalRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Searchable("Date")]
        [Displayable("Date", 1)]
        public DateTime Date { get; set; }

        [Searchable("Diagnosis")]
        [Displayable("Diagnosis", 2)]
        public string Diagnosis { get; set; } = string.Empty;

        [Displayable("Treatment", 3)]
        public string Treatment { get; set; } = string.Empty;

        [Displayable("Medications", 4)]
        public string Medications { get; set; } = string.Empty;

        [Displayable("Cost", 5)]
        public decimal Cost { get; set; }

        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public Appointment? Appointment { get; set; }
    }
}
