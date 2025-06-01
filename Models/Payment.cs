using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManager.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        public int MemberId { get; set; }

        public decimal Amount { get; set; }

        public string Method { get; set; }

        public DateTime Date { get; set; }

        // FOREIGN KEYS
        public virtual required Member Member { get; set; }
    }
}
