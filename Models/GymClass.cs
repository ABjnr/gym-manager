using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManager.Models
{
    public class GymClass
    {
        [Key]
        public int GymClassId { get; set; }

        public string Name { get; set; }

        public int TrainerId { get; set; }

        public TimeOnly ScheduleTime { get; set; }

        public int MaxCapacity { get; set; }

        // FOREIGN KEY
        public virtual required Trainer Trainer { get; set; }

        //many-to-many with Member
        public virtual ICollection<ClassRegistration>? ClassRegistrations { get; set; }

    }

    public class GymClassDto
    {
       
        public int GymClassId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int TrainerId { get; set; }

        public string? TrainerName { get; set; }

        [Required]
        public TimeOnly ScheduleTime { get; set; }

        [Required]
        public int MaxCapacity { get; set; }

    }

    public class GymClassDetailsViewModel
    {
        public GymClassDto GymClass { get; set; }
        public List<ClassRegistrationDto> EnrolledMembers { get; set; }
    }

}
