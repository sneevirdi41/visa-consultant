using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visa_consulatant.Data;
using visa_consulatant.Models;

namespace visa_consulatant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OffersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OffersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetOffers()
        {
            var offers = await _context.Offers
                .Include(o => o.ImageUpload)
                .Where(o => o.IsActive && (o.ValidUntil == null || o.ValidUntil > DateTime.UtcNow))
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new
                {
                    id = o.Id,
                    title = o.Title,
                    description = o.Description,
                    discountPercentage = o.DiscountPercentage,
                    validFrom = o.ValidFrom,
                    validUntil = o.ValidUntil,
                    imageUrl = o.ImageUploadId != null ? $"/api/image/{o.ImageUploadId}" : null,
                    createdAt = o.CreatedAt
                })
                .ToListAsync();

            return Ok(offers);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Offer>> CreateOffer([FromBody] CreateOfferRequest request)
        {
            var offer = new Offer
            {
                Title = request.Title,
                Description = request.Description,
                ImageUploadId = request.ImageUploadId,
                DiscountPercentage = request.DiscountPercentage,
                ValidFrom = request.ValidFrom,
                ValidUntil = request.ValidUntil,
                CreatedBy = User.Identity?.Name ?? "Admin",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Offers.Add(offer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOffers), new { id = offer.Id }, offer);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOffer(int id, [FromBody] UpdateOfferRequest request)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
                return NotFound();

            offer.Title = request.Title;
            offer.Description = request.Description;
            offer.ImageUploadId = request.ImageUploadId;
            offer.DiscountPercentage = request.DiscountPercentage;
            offer.ValidFrom = request.ValidFrom;
            offer.ValidUntil = request.ValidUntil;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
                return NotFound();

            offer.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class CreateOfferRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ImageUploadId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
    }

    public class UpdateOfferRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ImageUploadId { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
} 