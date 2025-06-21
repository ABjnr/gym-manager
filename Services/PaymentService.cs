using GymManager.Interfaces;
using GymManager.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentDto>> ListPayments()
        {
            return await _context.Payments
                .Include(p => p.Member)
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    MemberId = p.MemberId,
                    MemberFullName = p.Member.FirstName + " " + p.Member.LastName,
                    Amount = p.Amount,
                    Method = p.Method,
                    Date = p.Date
                })
                .ToListAsync();
        }

        public async Task<PaymentDto?> FindPayment(int id)
        {
            var p = await _context.Payments.Include(x => x.Member).FirstOrDefaultAsync(x => x.PaymentId == id);
            if (p == null) return null;
            return new PaymentDto
            {
                PaymentId = p.PaymentId,
                MemberId = p.MemberId,
                MemberFullName = p.Member.FirstName + " " + p.Member.LastName,
                Amount = p.Amount,
                Method = p.Method,
                Date = p.Date
            };
        }

        public async Task<ServiceResponse> AddPayment(PaymentDto dto)
        {
            var member = await _context.Members.FindAsync(dto.MemberId);
            if (member == null)
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound, Messages = new List<string> { "Member not found." } };

            var payment = new Payment
            {
                MemberId = dto.MemberId,
                Member = member,
                Amount = dto.Amount,
                Method = dto.Method,
                Date = dto.Date
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Created,
                CreatedId = payment.PaymentId,
                Messages = new List<string> { "Payment created successfully." }
            };
        }

        public async Task<ServiceResponse> UpdatePayment(PaymentDto dto)
        {
            var payment = await _context.Payments.FindAsync(dto.PaymentId);
            if (payment == null)
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound, Messages = new List<string> { "Payment not found." } };

            payment.MemberId = dto.MemberId;
            payment.Amount = dto.Amount;
            payment.Method = dto.Method;
            payment.Date = dto.Date;

            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Updated,
                CreatedId = payment.PaymentId,
                Messages = new List<string> { "Payment updated successfully." }
            };
        }

        public async Task<ServiceResponse> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound, Messages = new List<string> { "Payment not found." } };

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Deleted,
                CreatedId = id,
                Messages = new List<string> { "Payment deleted successfully." }
            };
        }
    }
}
