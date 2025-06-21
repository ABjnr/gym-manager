using GymManager.Models;

namespace GymManager.Interfaces
{
    public interface IClassRegistrationService
    {
        Task<IEnumerable<ClassRegistrationDto>> ListClassRegistrations();
        Task<ClassRegistrationDto?> FindClassRegistration(int id);
        Task<ServiceResponse> AddClassRegistration(ClassRegistrationDto dto);
        Task<ServiceResponse> UpdateClassRegistration(ClassRegistrationDto dto);
        Task<ServiceResponse> DeleteClassRegistration(int id);
    }
}
