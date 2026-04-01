using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VetClinic.Model.Attributes;

namespace VetClinic.Model.Entities
{
    public class Veterinarian
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Searchable("First Name")]
        [Displayable("First Name", 1)]
        public string FirstName { get; set; } = string.Empty;

        [Searchable("Last Name")]
        [Displayable("Last Name", 2)]
        public string LastName { get; set; } = string.Empty;

        [Searchable("Specialization")]
        [Displayable("Specialization", 3)]
        public string Specialization { get; set; } = string.Empty;

        [Displayable("License Number", 4)]
        public string LicenseNumber { get; set; } = string.Empty;

        [Displayable("Phone", 5)]
        public string Phone { get; set; } = string.Empty;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
