using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VetClinic.Model.Attributes;

namespace VetClinic.Model.Entities
{
    public class Owner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Displayable("ID", 0)]
        public int Id { get; set; }

        [Searchable("First Name")]
        [Displayable("First Name", 1)]
        public string FirstName { get; set; } = string.Empty;

        [Searchable("Last Name")]
        [Displayable("Last Name", 2)]
        public string LastName { get; set; } = string.Empty;

        [Searchable("Phone")]
        [Displayable("Phone", 3)]
        public string Phone { get; set; } = string.Empty;

        [Searchable("Email")]
        [Displayable("Email", 4)]
        public string Email { get; set; } = string.Empty;

        [Displayable("Address", 5)]
        public string Address { get; set; } = string.Empty;

        public ICollection<Dog> Dogs { get; set; } = new List<Dog>();
    }
}
