using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visa_consulatant.Data;
using visa_consulatant.Models;

namespace visa_consulatant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ImageController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ImageUpload>> UploadImage(IFormFile file, [FromForm] string imageType, [FromForm] string? title, [FromForm] string? description)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            if (file.Length > 10 * 1024 * 1024) // 10MB limit
                return BadRequest("File size too large. Maximum size is 10MB");

            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                return BadRequest("Invalid file type. Only JPEG, PNG, GIF, and WebP are allowed");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var imageData = memoryStream.ToArray();

            var imageUpload = new ImageUpload
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                ImageData = imageData,
                ImageType = imageType,
                Title = title,
                Description = description,
                UploadedBy = User.Identity?.Name ?? "Admin",
                UploadedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.ImageUploads.Add(imageUpload);
            await _context.SaveChangesAsync();

            return Ok(new { 
                id = imageUpload.Id, 
                fileName = imageUpload.FileName,
                imageType = imageUpload.ImageType,
                title = imageUpload.Title,
                uploadedAt = imageUpload.UploadedAt
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var image = await _context.ImageUploads.FindAsync(id);
            if (image == null || !image.IsActive)
                return NotFound();

            return File(image.ImageData, image.ContentType);
        }

        [HttpGet("logo")]
        public async Task<IActionResult> GetLogo()
        {
            var logo = await _context.ImageUploads
                .Where(i => i.ImageType == "logo" && i.IsActive)
                .OrderByDescending(i => i.UploadedAt)
                .FirstOrDefaultAsync();

            if (logo == null)
            {
                Console.WriteLine("No logo found in database");
                return NotFound();
            }

            Console.WriteLine($"Logo found: {logo.FileName}, ContentType: {logo.ContentType}, Size: {logo.ImageData.Length} bytes");
            return File(logo.ImageData, logo.ContentType);
        }

        [HttpGet("carousel")]
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
                    displayOrder = c.DisplayOrder
                })
                .ToListAsync();

            return Ok(carouselImages);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.ImageUploads.FindAsync(id);
            if (image == null)
                return NotFound();

            image.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 