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

        // GET: api/ClassRegistrations
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

        // GET: api/ClassRegistrations/5
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

        // PUT: api/ClassRegistrations/5
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

        // POST: api/ClassRegistrations
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


        // DELETE: api/ClassRegistrations/5
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
