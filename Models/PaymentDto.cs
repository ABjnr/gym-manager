using System.ComponentModel.DataAnnotations;

namespace GymManager.Models
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }

        [Required]
        public int MemberId { get; set; }

        public string? MemberFullName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be positive.")]
        public decimal Amount { get; set; }

        [Required]
        public string Method { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
