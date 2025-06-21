using GymManager.Models;

namespace GymManager.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> ListPayments();
        Task<PaymentDto?> FindPayment(int id);
        Task<ServiceResponse> AddPayment(PaymentDto dto);
        Task<ServiceResponse> UpdatePayment(PaymentDto dto);
        Task<ServiceResponse> DeletePayment(int id);
    }
}
