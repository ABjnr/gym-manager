using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManager.Models
{
    public class Trainer
    {
        [Key]
        public int TrainerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Specialization { get; set; }

        public virtual ICollection<GymClass>? GymClasses { get; set; }
    }
}
