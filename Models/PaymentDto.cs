using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManager.Models
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int MemberId { get; set; }
        public string MemberFullName { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public DateTime Date { get; set; }
    }
}
