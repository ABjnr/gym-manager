using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymManager.Models;

namespace GymManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all members with registration count.
        /// </summary>
        /// <returns>List of member DTOs.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers()
        {
            var memberDtos = await _context.Members
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

            return Ok(memberDtos);
        }

        /// <summary>
        /// Gets a specific member by ID with registration count.
        /// </summary>
        /// <param name="id">The member ID.</param>
        /// <returns>The member DTO if found; otherwise, NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetMember(int id)
        {
            var member = await _context.Members
                .Include(m => m.ClassRegistrations)
                .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member == null)
                return NotFound();

            var dto = new MemberDto
            {
                MemberId = member.MemberId,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                MembershipType = member.MembershipType,
                JoinDate = member.JoinDate,
                RegisteredClassCount = member.ClassRegistrations?.Count ?? 0
            };

            return Ok(dto);
        }

        /// <summary>
        /// Updates an existing member.
        /// </summary>
        /// <param name="id">The member ID.</param>
        /// <param name="dto">The updated member DTO.</param>
        /// <returns>NoContent if successful; otherwise, BadRequest or NotFound.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, MemberDto dto)
        {
            if (id != dto.MemberId)
                return BadRequest();

            var member = await _context.Members.FindAsync(id);
            if (member == null)
                return NotFound();

            member.FirstName = dto.FirstName;
            member.LastName = dto.LastName;
            member.Email = dto.Email;
            member.PhoneNumber = dto.PhoneNumber;
            member.MembershipType = dto.MembershipType;
            member.JoinDate = dto.JoinDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Creates a new member.
        /// </summary>
        /// <param name="dto">The member DTO to create.</param>
        /// <returns>The created member DTO.</returns>
        [HttpPost]
        public async Task<ActionResult<MemberDto>> PostMember(MemberDto dto)
        {
            var member = new Member
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                MembershipType = dto.MembershipType,
                JoinDate = dto.JoinDate
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            dto.MemberId = member.MemberId;
            return CreatedAtAction(nameof(GetMember), new { id = member.MemberId }, dto);
        }

        /// <summary>
        /// Deletes a member by ID.
        /// </summary>
        /// <param name="id">The member ID.</param>
        /// <returns>NoContent if successful; otherwise, NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
                return NotFound();

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}