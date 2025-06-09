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
    public class TrainersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Trainers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainerDto>>> GetTrainers()
        {
            var trainerDtos = await _context.Trainers
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

            return Ok(trainerDtos);
        }

        // GET: api/Trainers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainerDto>> GetTrainer(int id)
        {
            var t = await _context.Trainers.FindAsync(id);
            if (t == null)
                return NotFound();

            var dto = new TrainerDto
            {
                TrainerId = t.TrainerId,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber,
                Specialization = t.Specialization
            };

            return Ok(dto);
        }

        // PUT: api/Trainers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainer(int id, TrainerDto dto)
        {
            if (id != dto.TrainerId)
                return BadRequest();

            var t = await _context.Trainers.FindAsync(id);
            if (t == null)
                return NotFound();

            t.FirstName = dto.FirstName;
            t.LastName = dto.LastName;
            t.Email = dto.Email;
            t.PhoneNumber = dto.PhoneNumber;
            t.Specialization = dto.Specialization;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Trainers
        [HttpPost]
        public async Task<ActionResult<TrainerDto>> PostTrainer(TrainerDto dto)
        {
            var t = new Trainer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Specialization = dto.Specialization
            };

            _context.Trainers.Add(t);
            await _context.SaveChangesAsync();

            dto.TrainerId = t.TrainerId;
            return CreatedAtAction(nameof(GetTrainer), new { id = t.TrainerId }, dto);
        }

        // DELETE: api/Trainers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var t = await _context.Trainers.FindAsync(id);
            if (t == null)
                return NotFound();

            _context.Trainers.Remove(t);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
