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
    public class ClassRegistrationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClassRegistrationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all class registrations with member and gym class details.
        /// </summary>
        /// <returns>List of class registration DTOs.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassRegistrationDto>>> GetClassRegistrations()
        {
            var dtos = await _context.ClassRegistrations
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

            return Ok(dtos);
        }

        /// <summary>
        /// Gets a specific class registration by ID.
        /// </summary>
        /// <param name="id">The class registration ID.</param>
        /// <returns>The class registration DTO if found; otherwise, NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassRegistrationDto>> GetClassRegistration(int id)
        {
            var r = await _context.ClassRegistrations
                .Include(x => x.Member)
                .Include(x => x.GymClass)
                .FirstOrDefaultAsync(x => x.ClassRegistrationId == id);

            if (r == null)
                return NotFound();

            var dto = new ClassRegistrationDto
            {
                ClassRegistrationId = r.ClassRegistrationId,
                MemberId = r.MemberId,
                MemberFullName = r.Member.FirstName + " " + r.Member.LastName,
                GymClassId = r.GymClassId,
                GymClassName = r.GymClass.Name,
                RegistrationDate = r.RegistrationDate
            };

            return Ok(dto);
        }

        /// <summary>
        /// Updates an existing class registration.
        /// </summary>
        /// <param name="id">The class registration ID.</param>
        /// <param name="dto">The updated class registration DTO.</param>
        /// <returns>NoContent if successful; otherwise, BadRequest or NotFound.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassRegistration(int id, ClassRegistrationDto dto)
        {
            if (id != dto.ClassRegistrationId)
                return BadRequest();

            var r = await _context.ClassRegistrations.FindAsync(id);
            if (r == null)
                return NotFound();

            r.MemberId = dto.MemberId;
            r.GymClassId = dto.GymClassId;
            r.RegistrationDate = dto.RegistrationDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Creates a new class registration.
        /// </summary>
        /// <param name="dto">The class registration DTO to create.</param>
        /// <returns>The created class registration DTO.</returns>
        [HttpPost]
        public async Task<ActionResult<ClassRegistrationDto>> PostClassRegistration(ClassRegistrationDto dto)
        {
            var member = await _context.Members.FindAsync(dto.MemberId);
            if (member == null)
                return BadRequest("Member not found.");

            var gymClass = await _context.gymClasses.FindAsync(dto.GymClassId);
            if (gymClass == null)
                return BadRequest("Gym class not found.");

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

            dto.ClassRegistrationId = r.ClassRegistrationId;
            return CreatedAtAction(nameof(GetClassRegistration), new { id = r.ClassRegistrationId }, dto);
        }

        /// <summary>
        /// Deletes a class registration by ID.
        /// </summary>
        /// <param name="id">The class registration ID.</param>
        /// <returns>NoContent if successful; otherwise, NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassRegistration(int id)
        {
            var r = await _context.ClassRegistrations.FindAsync(id);
            if (r == null)
                return NotFound();

            _context.ClassRegistrations.Remove(r);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}