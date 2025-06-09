using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymManager.Models;
using NuGet.DependencyResolver;

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
        public async Task<ActionResult<IEnumerable<GymClassDto>>> GetGymClasses()
        {
            var dtos = await _context.gymClasses
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

            return Ok(dtos);
        }

        // GET: api/GymClasses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GymClassDto>> GetGymClass(int id)
        {
            var g = await _context.gymClasses
                .Include(gc => gc.Trainer)
                .FirstOrDefaultAsync(gc => gc.GymClassId == id);

            if (g == null)
                return NotFound();

            var dto = new GymClassDto
            {
                GymClassId = g.GymClassId,
                Name = g.Name,
                TrainerId = g.TrainerId,
                TrainerName = g.Trainer.FirstName + " " + g.Trainer.LastName,
                ScheduleTime = g.ScheduleTime,
                MaxCapacity = g.MaxCapacity
            };

            return Ok(dto);
        }

        // PUT: api/GymClasses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGymClass(int id, GymClassDto dto)
        {
            if (id != dto.GymClassId)
                return BadRequest();

            var g = await _context.gymClasses.FindAsync(id);
            if (g == null)
                return NotFound();

            g.Name = dto.Name;
            g.TrainerId = dto.TrainerId;
            g.ScheduleTime = dto.ScheduleTime;
            g.MaxCapacity = dto.MaxCapacity;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/GymClasses
        [HttpPost]
        public async Task<ActionResult<GymClassDto>> PostGymClass(GymClassDto dto)
        {
            var trainer = await _context.Trainers.FindAsync(dto.TrainerId);
            if (trainer == null)
                return BadRequest("Trainer not found.");

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

            dto.GymClassId = g.GymClassId;
            return CreatedAtAction(nameof(GetGymClass), new { id = g.GymClassId }, dto);
        }


        // DELETE: api/GymClasses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGymClass(int id)
        {
            var g = await _context.gymClasses.FindAsync(id);
            if (g == null)
                return NotFound();

            _context.gymClasses.Remove(g);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
