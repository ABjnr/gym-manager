using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManager.Models
{
    public class ClassRegistration
    {
        [Key]
        public int ClassRegistrationId { get; set; }

        public int MemberId { get; set; }

        public int GymClassId { get; set; }

        public DateTime RegistrationDate { get; set; }

        // FOREIGN KEYS
        public virtual required Member Member { get; set; }

        public virtual required GymClass GymClass { get; set; }

    }
}
