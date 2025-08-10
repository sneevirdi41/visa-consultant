using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visa_consulatant.Data;
using visa_consulatant.Models;

namespace visa_consulatant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarouselController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarouselController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCarouselImages()
        {
            var carouselImages = await _context.CarouselImages
                .Include(c => c.ImageUpload)
                .Where(c => c.IsActive && c.ImageUpload != null && c.ImageUpload.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.CreatedAt)
                .Select(c => new
                {
                    id = c.Id,
                    title = c.Title,
                    description = c.Description,
                    imageUrl = $"/api/image/{c.ImageUploadId}",
                    displayOrder = c.DisplayOrder,
                    createdAt = c.CreatedAt
                })
                .ToListAsync();

            return Ok(carouselImages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCarouselImage(int id)
        {
            var carouselImage = await _context.CarouselImages
                .Include(c => c.ImageUpload)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            if (carouselImage == null)
                return NotFound();

            return Ok(new
            {
                id = carouselImage.Id,
                title = carouselImage.Title,
                description = carouselImage.Description,
                imageUrl = $"/api/image/{carouselImage.ImageUploadId}",
                displayOrder = carouselImage.DisplayOrder,
                createdAt = carouselImage.CreatedAt
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CarouselImage>> CreateCarouselImage([FromBody] CreateCarouselImageRequest request)
        {
            var imageUpload = await _context.ImageUploads.FindAsync(request.ImageUploadId);
            if (imageUpload == null)
                return BadRequest("Image not found");

            var carouselImage = new CarouselImage
            {
                Title = request.Title,
                Description = request.Description,
                ImageUploadId = request.ImageUploadId,
                DisplayOrder = request.DisplayOrder,
                CreatedBy = User.Identity?.Name ?? "Admin",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.CarouselImages.Add(carouselImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCarouselImages), new { id = carouselImage.Id }, carouselImage);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCarouselImage(int id, [FromBody] UpdateCarouselImageRequest request)
        {
            var carouselImage = await _context.CarouselImages.FindAsync(id);
            if (carouselImage == null)
                return NotFound();

            carouselImage.Title = request.Title;
            carouselImage.Description = request.Description;
            carouselImage.DisplayOrder = request.DisplayOrder;
            
            // Update image if new one provided
            if (request.ImageUploadId.HasValue)
            {
                var imageUpload = await _context.ImageUploads.FindAsync(request.ImageUploadId.Value);
                if (imageUpload == null)
                    return BadRequest("Image not found");
                    
                carouselImage.ImageUploadId = request.ImageUploadId.Value;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCarouselImage(int id)
        {
            var carouselImage = await _context.CarouselImages.FindAsync(id);
            if (carouselImage == null)
                return NotFound();

            carouselImage.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class CreateCarouselImageRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ImageUploadId { get; set; }
        public int DisplayOrder { get; set; } = 0;
    }

    public class UpdateCarouselImageRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public int? ImageUploadId { get; set; } // Optional for updating image
    }
} 