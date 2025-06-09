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
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all payments with member details.
        /// </summary>
        /// <returns>List of payment DTOs.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments()
        {
            var dtos = await _context.Payments
                .Include(p => p.Member)
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    MemberId = p.MemberId,
                    MemberFullName = p.Member.FirstName + " " + p.Member.LastName,
                    Amount = p.Amount,
                    Method = p.Method,
                    Date = p.Date
                })
                .ToListAsync();

            return Ok(dtos);
        }

        /// <summary>
        /// Gets a specific payment by ID.
        /// </summary>
        /// <param name="id">The payment ID.</param>
        /// <returns>The payment DTO if found; otherwise, NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetPayment(int id)
        {
            var p = await _context.Payments
                .Include(x => x.Member)
                .FirstOrDefaultAsync(x => x.PaymentId == id);

            if (p == null)
                return NotFound();

            var dto = new PaymentDto
            {
                PaymentId = p.PaymentId,
                MemberId = p.MemberId,
                MemberFullName = p.Member.FirstName + " " + p.Member.LastName,
                Amount = p.Amount,
                Method = p.Method,
                Date = p.Date
            };

            return Ok(dto);
        }

        /// <summary>
        /// Updates an existing payment.
        /// </summary>
        /// <param name="id">The payment ID.</param>
        /// <param name="dto">The updated payment DTO.</param>
        /// <returns>NoContent if successful; otherwise, BadRequest or NotFound.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, PaymentDto dto)
        {
            if (id != dto.PaymentId)
                return BadRequest();

            var p = await _context.Payments.FindAsync(id);
            if (p == null)
                return NotFound();

            p.MemberId = dto.MemberId;
            p.Amount = dto.Amount;
            p.Method = dto.Method;
            p.Date = dto.Date;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Creates a new payment.
        /// </summary>
        /// <param name="dto">The payment DTO to create.</param>
        /// <returns>The created payment DTO.</returns>
        [HttpPost]
        public async Task<ActionResult<PaymentDto>> PostPayment(PaymentDto dto)
        {
            var member = await _context.Members.FindAsync(dto.MemberId);
            if (member == null)
                return BadRequest("Member not found.");

            var p = new Payment
            {
                MemberId = dto.MemberId,
                Member = member,
                Amount = dto.Amount,
                Method = dto.Method,
                Date = dto.Date
            };

            _context.Payments.Add(p);
            await _context.SaveChangesAsync();

            dto.PaymentId = p.PaymentId;
            return CreatedAtAction(nameof(GetPayment), new { id = p.PaymentId }, dto);
        }

        /// <summary>
        /// Deletes a payment by ID.
        /// </summary>
        /// <param name="id">The payment ID.</param>
        /// <returns>NoContent if successful; otherwise, NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var p = await _context.Payments.FindAsync(id);
            if (p == null)
                return NotFound();

            _context.Payments.Remove(p);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
