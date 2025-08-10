using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visa_consulatant.Data;
using visa_consulatant.Models;
using visa_consulatant.Models.DTOs;

namespace visa_consulatant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactInfoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContactInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/contactinfo
        [HttpGet]
        public async Task<ActionResult<ContactInfoDto>> GetContactInfo()
        {
            var contactInfo = await _context.ContactInfos
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt)
                .FirstOrDefaultAsync();

            if (contactInfo == null)
            {
                return NotFound("No contact information found");
            }

            var dto = new ContactInfoDto
            {
                Id = contactInfo.Id,
                CompanyName = contactInfo.CompanyName,
                Address = contactInfo.Address,
                City = contactInfo.City,
                State = contactInfo.State,
                PostalCode = contactInfo.PostalCode,
                Country = contactInfo.Country,
                Phone = contactInfo.Phone,
                Mobile = contactInfo.Mobile,
                Email = contactInfo.Email,
                Website = contactInfo.Website,
                WorkingHours = contactInfo.WorkingHours,
                Description = contactInfo.Description,
                IsActive = contactInfo.IsActive,
                CreatedAt = contactInfo.CreatedAt,
                UpdatedAt = contactInfo.UpdatedAt
            };

            return Ok(dto);
        }

        // GET: api/contactinfo/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ContactInfoDto>>> GetAllContactInfo()
        {
            var contactInfos = await _context.ContactInfos
                .OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt)
                .ToListAsync();

            var dtos = contactInfos.Select(c => new ContactInfoDto
            {
                Id = c.Id,
                CompanyName = c.CompanyName,
                Address = c.Address,
                City = c.City,
                State = c.State,
                PostalCode = c.PostalCode,
                Country = c.Country,
                Phone = c.Phone,
                Mobile = c.Mobile,
                Email = c.Email,
                Website = c.Website,
                WorkingHours = c.WorkingHours,
                Description = c.Description,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            });

            return Ok(dtos);
        }

        // POST: api/contactinfo
        [HttpPost]
        public async Task<ActionResult<ContactInfoDto>> CreateContactInfo([FromBody] UpdateContactInfoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Deactivate all existing contact info
            var existingContactInfos = await _context.ContactInfos.ToListAsync();
            foreach (var existing in existingContactInfos)
            {
                existing.IsActive = false;
            }

            var contactInfo = new ContactInfo
            {
                CompanyName = request.CompanyName,
                Address = request.Address,
                City = request.City,
                State = request.State,
                PostalCode = request.PostalCode,
                Country = request.Country,
                Phone = request.Phone,
                Mobile = request.Mobile,
                Email = request.Email,
                Website = request.Website,
                WorkingHours = request.WorkingHours,
                Description = request.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.ContactInfos.Add(contactInfo);
            await _context.SaveChangesAsync();

            var dto = new ContactInfoDto
            {
                Id = contactInfo.Id,
                CompanyName = contactInfo.CompanyName,
                Address = contactInfo.Address,
                City = contactInfo.City,
                State = contactInfo.State,
                PostalCode = contactInfo.PostalCode,
                Country = contactInfo.Country,
                Phone = contactInfo.Phone,
                Mobile = contactInfo.Mobile,
                Email = contactInfo.Email,
                Website = contactInfo.Website,
                WorkingHours = contactInfo.WorkingHours,
                Description = contactInfo.Description,
                IsActive = contactInfo.IsActive,
                CreatedAt = contactInfo.CreatedAt,
                UpdatedAt = contactInfo.UpdatedAt
            };

            return CreatedAtAction(nameof(GetContactInfo), new { id = contactInfo.Id }, dto);
        }

        // PUT: api/contactinfo/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContactInfo(int id, [FromBody] UpdateContactInfoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactInfo = await _context.ContactInfos.FindAsync(id);
            if (contactInfo == null)
            {
                return NotFound($"Contact info with ID {id} not found");
            }

            contactInfo.CompanyName = request.CompanyName;
            contactInfo.Address = request.Address;
            contactInfo.City = request.City;
            contactInfo.State = request.State;
            contactInfo.PostalCode = request.PostalCode;
            contactInfo.Country = request.Country;
            contactInfo.Phone = request.Phone;
            contactInfo.Mobile = request.Mobile;
            contactInfo.Email = request.Email;
            contactInfo.Website = request.Website;
            contactInfo.WorkingHours = request.WorkingHours;
            contactInfo.Description = request.Description;
            contactInfo.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactInfoExists(id))
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

        // DELETE: api/contactinfo/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactInfo(int id)
        {
            var contactInfo = await _context.ContactInfos.FindAsync(id);
            if (contactInfo == null)
            {
                return NotFound($"Contact info with ID {id} not found");
            }

            contactInfo.IsActive = false;
            contactInfo.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactInfoExists(int id)
        {
            return _context.ContactInfos.Any(e => e.Id == id);
        }
    }
}

