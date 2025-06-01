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
    public class GymClassesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GymClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/GymClasses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GymClass>>> GetgymClasses()
        {
            return await _context.gymClasses.ToListAsync();
        }

        // GET: api/GymClasses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GymClass>> GetGymClass(int id)
        {
            var gymClass = await _context.gymClasses.FindAsync(id);

            if (gymClass == null)
            {
                return NotFound();
            }

            return gymClass;
        }

        // PUT: api/GymClasses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGymClass(int id, GymClass gymClass)
        {
            if (id != gymClass.GymClassId)
            {
                return BadRequest();
            }

            _context.Entry(gymClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GymClassExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GymClasses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GymClass>> PostGymClass(GymClass gymClass)
        {
            _context.gymClasses.Add(gymClass);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGymClass", new { id = gymClass.GymClassId }, gymClass);
        }

        // DELETE: api/GymClasses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGymClass(int id)
        {
            var gymClass = await _context.gymClasses.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }

            _context.gymClasses.Remove(gymClass);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GymClassExists(int id)
        {
            return _context.gymClasses.Any(e => e.GymClassId == id);
        }
    }
}
