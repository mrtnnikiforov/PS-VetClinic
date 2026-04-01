using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VetClinic.Model.Attributes;

namespace VetClinic.Model.Entities
{
    public class Dog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Searchable("Name")]
        [Displayable("Name", 1)]
        public string Name { get; set; } = string.Empty;

        [Searchable("Breed")]
        [Displayable("Breed", 2)]
        public string Breed { get; set; } = string.Empty;

        [Searchable("Date of Birth")]
        [Displayable("Date of Birth", 3)]
        public DateTime DateOfBirth { get; set; }

        [Displayable("Weight (kg)", 4)]
        public double WeightKg { get; set; }

        [Searchable("Chip Number")]
        [Displayable("Chip Number", 5)]
        public string ChipNumber { get; set; } = string.Empty;

        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public Owner? Owner { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
