using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManager.Models
{
    public class Member
    {
        [Key]
        public int MemberId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string MembershipType { get; set; }

        public DateTime JoinDate { get; set; }

        // A CLASS HAS MULTIPLE MEMBERS -  many-to-many with GymClass
        public ICollection<ClassRegistration>? ClassRegistrations { get; set; }
    }

    public class MemberDto
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MembershipType { get; set; }
        public DateTime JoinDate { get; set; }
        public int RegisteredClassCount { get; set; }
    }

}
