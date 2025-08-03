using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visa_consulatant.Data;
using visa_consulatant.Models;

namespace visa_consulatant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisaServicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VisaServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisaService>>> GetVisaServices()
        {
            return await _context.VisaServices
                .Where(v => v.IsActive)
                .OrderBy(v => v.Name)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisaService>> GetVisaService(int id)
        {
            var visaService = await _context.VisaServices.FindAsync(id);

            if (visaService == null || !visaService.IsActive)
            {
                return NotFound();
            }

            return visaService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<VisaService>> CreateVisaService(VisaService visaService)
        {
            visaService.CreatedAt = DateTime.UtcNow;
            visaService.IsActive = true;

            _context.VisaServices.Add(visaService);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVisaService), new { id = visaService.Id }, visaService);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVisaService(int id, VisaService visaService)
        {
            if (id != visaService.Id)
            {
                return BadRequest();
            }

            var existingService = await _context.VisaServices.FindAsync(id);
            if (existingService == null)
            {
                return NotFound();
            }

            existingService.Name = visaService.Name;
            existingService.Description = visaService.Description;
            existingService.ImageUrl = visaService.ImageUrl;
            existingService.Price = visaService.Price;
            existingService.ProcessingTime = visaService.ProcessingTime;
            existingService.Country = visaService.Country;
            existingService.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisaServiceExists(id))
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVisaService(int id)
        {
            var visaService = await _context.VisaServices.FindAsync(id);
            if (visaService == null)
            {
                return NotFound();
            }

            visaService.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VisaServiceExists(int id)
        {
            return _context.VisaServices.Any(e => e.Id == id);
        }
    }
} 