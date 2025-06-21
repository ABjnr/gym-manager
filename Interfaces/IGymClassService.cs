using GymManager.Models;

namespace GymManager.Interfaces
{
    public interface IGymClassService
    {
        Task<IEnumerable<GymClassDto>> ListGymClasses();
        Task<GymClassDto?> FindGymClass(int id);
        Task<ServiceResponse> AddGymClass(GymClassDto gymClassDto);
        Task<ServiceResponse> UpdateGymClass(GymClassDto gymClassDto);
        Task<ServiceResponse> DeleteGymClass(int id);
    }
}
