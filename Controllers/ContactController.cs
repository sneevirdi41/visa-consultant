using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visa_consulatant.Data;
using visa_consulatant.Models;

namespace visa_consulatant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("inquiry")]
        public async Task<ActionResult<ContactInquiry>> SubmitInquiry(ContactInquiry inquiry)
        {
            inquiry.CreatedAt = DateTime.UtcNow;
            inquiry.IsRead = false;

            _context.ContactInquiries.Add(inquiry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInquiry), new { id = inquiry.Id }, inquiry);
        }

        [HttpGet("inquiries")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ContactInquiry>>> GetInquiries()
        {
            return await _context.ContactInquiries
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        [HttpGet("inquiries/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ContactInquiry>> GetInquiry(int id)
        {
            var inquiry = await _context.ContactInquiries.FindAsync(id);

            if (inquiry == null)
            {
                return NotFound();
            }

            return inquiry;
        }

        [HttpPut("inquiries/{id}/mark-read")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var inquiry = await _context.ContactInquiries.FindAsync(id);

            if (inquiry == null)
            {
                return NotFound();
            }

            inquiry.IsRead = true;
            inquiry.ReadAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("inquiries/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInquiry(int id)
        {
            var inquiry = await _context.ContactInquiries.FindAsync(id);

            if (inquiry == null)
            {
                return NotFound();
            }

            _context.ContactInquiries.Remove(inquiry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 