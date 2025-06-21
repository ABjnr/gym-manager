using GymManager.Interfaces;
using GymManager.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Services
{
    public class ClassRegistrationService : IClassRegistrationService
    {
        private readonly ApplicationDbContext _context;

        public ClassRegistrationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClassRegistrationDto>> ListClassRegistrations()
        {
            return await _context.ClassRegistrations
                .Include(r => r.Member)
                .Include(r => r.GymClass)
                .Select(r => new ClassRegistrationDto
                {
                    ClassRegistrationId = r.ClassRegistrationId,
                    MemberId = r.MemberId,
                    MemberFullName = r.Member.FirstName + " " + r.Member.LastName,
                    GymClassId = r.GymClassId,
                    GymClassName = r.GymClass.Name,
                    RegistrationDate = r.RegistrationDate
                })
                .ToListAsync();
        }

        public async Task<ClassRegistrationDto?> FindClassRegistration(int id)
        {
            var r = await _context.ClassRegistrations
                .Include(x => x.Member)
                .Include(x => x.GymClass)
                .FirstOrDefaultAsync(x => x.ClassRegistrationId == id);

            if (r == null) return null;

            return new ClassRegistrationDto
            {
                ClassRegistrationId = r.ClassRegistrationId,
                MemberId = r.MemberId,
                MemberFullName = r.Member.FirstName + " " + r.Member.LastName,
                GymClassId = r.GymClassId,
                GymClassName = r.GymClass.Name,
                RegistrationDate = r.RegistrationDate
            };
        }

        public async Task<ServiceResponse> AddClassRegistration(ClassRegistrationDto dto)
        {
            var member = await _context.Members.FindAsync(dto.MemberId);
            if (member == null)
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound, Messages = new List<string> { "Member not found." } };

            var gymClass = await _context.gymClasses.FindAsync(dto.GymClassId);
            if (gymClass == null)
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound, Messages = new List<string> { "Gym class not found." } };

            var r = new ClassRegistration
            {
                MemberId = dto.MemberId,
                Member = member,
                GymClassId = dto.GymClassId,
                GymClass = gymClass,
                RegistrationDate = dto.RegistrationDate
            };

            _context.ClassRegistrations.Add(r);
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Created,
                CreatedId = r.ClassRegistrationId,
                Messages = new List<string> { "Class registration created successfully." }
            };
        }

        public async Task<ServiceResponse> UpdateClassRegistration(ClassRegistrationDto dto)
        {
            var r = await _context.ClassRegistrations.FindAsync(dto.ClassRegistrationId);
            if (r == null)
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound, Messages = new List<string> { "Registration not found." } };

            r.MemberId = dto.MemberId;
            r.GymClassId = dto.GymClassId;
            r.RegistrationDate = dto.RegistrationDate;

            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Updated,
                CreatedId = r.ClassRegistrationId,
                Messages = new List<string> { "Class registration updated successfully." }
            };
        }

        public async Task<ServiceResponse> DeleteClassRegistration(int id)
        {
            var r = await _context.ClassRegistrations.FindAsync(id);
            if (r == null)
                return new ServiceResponse { Status = ServiceResponse.ServiceStatus.NotFound, Messages = new List<string> { "Registration not found." } };

            _context.ClassRegistrations.Remove(r);
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Deleted,
                CreatedId = id,
                Messages = new List<string> { "Class registration deleted successfully." }
            };
        }
    }
}
