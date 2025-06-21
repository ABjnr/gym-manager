using GymManager.Models;

namespace GymManager.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDto>> ListMembers();
        Task<MemberDto?> FindMember(int id);
        Task<ServiceResponse> UpdateMember(MemberDto memberDto);
        Task<ServiceResponse> AddMember(MemberDto memberDto);
        Task<ServiceResponse> DeleteMember(int id);

        // Related: List all classes a member is registered for
        Task<IEnumerable<ClassRegistrationDto>> ListRegistrationsForMember(int memberId);
    }
}
