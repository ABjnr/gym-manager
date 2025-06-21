using GymManager.Interfaces;
using GymManager.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Services
{
    public class TrainerService : ITrainerService
    {
        private readonly ApplicationDbContext _context;

        public TrainerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrainerDto>> ListTrainers()
        {
            return await _context.Trainers
                .Select(t => new TrainerDto
                {
                    TrainerId = t.TrainerId,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber,
                    Specialization = t.Specialization
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<TrainerDto>> GetAllTrainers()
        {
            var trainers = await _context.Trainers
                .Select(t => new TrainerDto
                {
                    TrainerId = t.TrainerId,
                    FirstName = t.FirstName,
                    LastName = t.LastName
                }).ToListAsync();

            return trainers;
        }


        public async Task<TrainerDto?> FindTrainer(int id)
        {
            var t = await _context.Trainers.FindAsync(id);
            if (t == null) return null;

            return new TrainerDto
            {
                TrainerId = t.TrainerId,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber,
                Specialization = t.Specialization
            };
        }

        public async Task<ServiceResponse> UpdateTrainer(TrainerDto trainerDto)
        {
            var t = await _context.Trainers.FindAsync(trainerDto.TrainerId);
            if (t == null)
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.NotFound,
                    Messages = new List<string> { "Trainer not found." }
                };
            }

            t.FirstName = trainerDto.FirstName;
            t.LastName = trainerDto.LastName;
            t.Email = trainerDto.Email;
            t.PhoneNumber = trainerDto.PhoneNumber;
            t.Specialization = trainerDto.Specialization;

            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Updated,
                CreatedId = t.TrainerId,
                Messages = new List<string> { "Trainer updated successfully." }
            };
        }

        public async Task<ServiceResponse> AddTrainer(TrainerDto trainerDto)
        {
            var t = new Trainer
            {
                FirstName = trainerDto.FirstName,
                LastName = trainerDto.LastName,
                Email = trainerDto.Email,
                PhoneNumber = trainerDto.PhoneNumber,
                Specialization = trainerDto.Specialization
            };

            _context.Trainers.Add(t);
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Created,
                CreatedId = t.TrainerId,
                Messages = new List<string> { "Trainer created successfully." }
            };
        }

        public async Task<ServiceResponse> DeleteTrainer(int id)
        {
            var t = await _context.Trainers.FindAsync(id);
            if (t == null)
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.NotFound,
                    Messages = new List<string> { "Trainer not found." }
                };
            }

            _context.Trainers.Remove(t);
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Deleted,
                CreatedId = id,
                Messages = new List<string> { "Trainer deleted successfully." }
            };
        }

        public async Task<IEnumerable<GymClassDto>> ListClassesForTrainer(int trainerId)
        {
            return await _context.gymClasses
                .Where(g => g.TrainerId == trainerId)
                .Select(g => new GymClassDto
                {
                    GymClassId = g.GymClassId,
                    Name = g.Name,
                    TrainerId = g.TrainerId,
                    TrainerName = g.Trainer.FirstName + " " + g.Trainer.LastName,
                    ScheduleTime = g.ScheduleTime,
                    MaxCapacity = g.MaxCapacity
                })
                .ToListAsync();
        }
    }
}
