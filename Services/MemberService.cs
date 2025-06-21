using GymManager.Interfaces;
using GymManager.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Services
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContext _context;

        public MemberService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MemberDto>> ListMembers()
        {
            return await _context.Members
                .Select(m => new MemberDto
                {
                    MemberId = m.MemberId,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Email = m.Email,
                    PhoneNumber = m.PhoneNumber,
                    MembershipType = m.MembershipType,
                    JoinDate = m.JoinDate,
                    RegisteredClassCount = m.ClassRegistrations.Count()
                })
                .ToListAsync();
        }

        public async Task<MemberDto?> FindMember(int id)
        {
            var m = await _context.Members
                .Include(x => x.ClassRegistrations)
                .FirstOrDefaultAsync(x => x.MemberId == id);

            if (m == null) return null;

            return new MemberDto
            {
                MemberId = m.MemberId,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Email = m.Email,
                PhoneNumber = m.PhoneNumber,
                MembershipType = m.MembershipType,
                JoinDate = m.JoinDate,
                RegisteredClassCount = m.ClassRegistrations?.Count ?? 0
            };
        }

        public async Task<ServiceResponse> UpdateMember(MemberDto memberDto)
        {
            var member = await _context.Members.FindAsync(memberDto.MemberId);
            if (member == null)
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.NotFound,
                    Messages = new List<string> { "Member not found." }
                };
            }

            member.FirstName = memberDto.FirstName;
            member.LastName = memberDto.LastName;
            member.Email = memberDto.Email;
            member.PhoneNumber = memberDto.PhoneNumber;
            member.MembershipType = memberDto.MembershipType;
            member.JoinDate = memberDto.JoinDate;

            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Updated,
                CreatedId = member.MemberId,
                Messages = new List<string> { "Member updated successfully." }
            };
        }

        public async Task<ServiceResponse> AddMember(MemberDto memberDto)
        {
            Console.WriteLine("addMemberPage");
            var member = new Member
            {
                FirstName = memberDto.FirstName,
                LastName = memberDto.LastName,
                Email = memberDto.Email,
                PhoneNumber = memberDto.PhoneNumber,
                MembershipType = memberDto.MembershipType,
                JoinDate = memberDto.JoinDate
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Created,
                CreatedId = member.MemberId,
                Messages = new List<string> { "Member created successfully." }
            };
        }

        public async Task<ServiceResponse> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return new ServiceResponse
                {
                    Status = ServiceResponse.ServiceStatus.NotFound,
                    Messages = new List<string> { "Member not found." }
                };
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return new ServiceResponse
            {
                Status = ServiceResponse.ServiceStatus.Deleted,
                CreatedId = id,
                Messages = new List<string> { "Member deleted successfully." }
            };
        }

        public async Task<IEnumerable<ClassRegistrationDto>> ListRegistrationsForMember(int memberId)
        {
            return await _context.ClassRegistrations
                .Include(r => r.GymClass)
                .Where(r => r.MemberId == memberId)
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
    }
}
