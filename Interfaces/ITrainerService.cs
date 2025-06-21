using GymManager.Models;

namespace GymManager.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerDto>> ListTrainers();
        Task<TrainerDto?> FindTrainer(int id);
        Task<ServiceResponse> UpdateTrainer(TrainerDto trainerDto);
        Task<ServiceResponse> AddTrainer(TrainerDto trainerDto);
        Task<ServiceResponse> DeleteTrainer(int id);

        // Related: List all classes taught by a trainer
        Task<IEnumerable<GymClassDto>> ListClassesForTrainer(int trainerId);
    }
}
