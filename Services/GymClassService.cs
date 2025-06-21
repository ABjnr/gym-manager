using GymManager.Interfaces;
using GymManager.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Services
{
    public class GymClassService : IGymClassService
    {
        private readonly ApplicationDbContext _context;

        public GymClassService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GymClassDto>> ListGymClasses()
        {
            return await _context.gymClasses
                .Include(g => g.Trainer)
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

        public async Task<GymClassDto?> FindGymClass(int id)
        {
            var g = await _context.gymClasses.Include(x => x.Trainer).FirstOrDefaultAsync(x => x.GymClassId == id);
            if (g == null) return null;
            return new GymClassDto
            {
                GymClassId = g.GymClassId,
                Name = g.Name,
                TrainerId = g.TrainerId,
                TrainerName = g.Trainer.FirstName + " " + g.Trainer.LastName,
                ScheduleTime = g.ScheduleTime,
                MaxCapacity = g.MaxCapacity
            };
        }

        public async Task<ServiceResponse> AddGymClass(GymClassDto dto)
        {
            var trainer = await _context.Trainers.FindAsync(dto.TrainerId);
            if (trainer == null)
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.NotFound,
                    Messages = new List<string> { "Trainer not found." }
                };
            }

            var g = new GymClass
            {
                Name = dto.Name,
                TrainerId = dto.TrainerId,
                Trainer = trainer,
                ScheduleTime = dto.ScheduleTime,
                MaxCapacity = dto.MaxCapacity
            };
            _context.gymClasses.Add(g);
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Created,
                CreatedId = g.GymClassId,
                Messages = new List<string> { "Gym class created successfully." }
            };
        }

        public async Task<ServiceResponse> UpdateGymClass(GymClassDto dto)
        {
            var g = await _context.gymClasses.FindAsync(dto.GymClassId);
            if (g == null)
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.NotFound,
                    Messages = new List<string> { "Gym class not found." }
                };
            }
            g.Name = dto.Name;
            g.TrainerId = dto.TrainerId;
            g.ScheduleTime = dto.ScheduleTime;
            g.MaxCapacity = dto.MaxCapacity;
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Updated,
                CreatedId = g.GymClassId,
                Messages = new List<string> { "Gym class updated successfully." }
            };
        }

        public async Task<ServiceResponse> DeleteGymClass(int id)
        {
            var g = await _context.gymClasses.FindAsync(id);
            if (g == null)
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.NotFound,
                    Messages = new List<string> { "Gym class not found." }
                };
            }
            _context.gymClasses.Remove(g);
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Deleted,
                CreatedId = id,
                Messages = new List<string> { "Gym class deleted successfully." }
            };
        }
    }
}
