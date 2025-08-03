using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visa_consulatant.Data;
using visa_consulatant.Models;
using visa_consulatant.Models.DTOs;

namespace visa_consulatant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomePageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HomePageController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<HomePageContentDto>> GetHomePageContent()
        {
            var content = await _context.HomePageContents.FirstOrDefaultAsync();
            
            if (content == null)
            {
                return NotFound(new { message = "Homepage content not found" });
            }

            return Ok(new HomePageContentDto
            {
                Id = content.Id,
                Title = content.Title,
                Description = content.Description,
                HeroImageUrl = content.HeroImageUrl,
                LogoUrl = content.LogoUrl,
                BannerImageUrl = content.BannerImageUrl,
                WelcomeMessage = content.WelcomeMessage,
                ServicesOverview = content.ServicesOverview,
                LastUpdated = content.LastUpdated,
                UpdatedBy = content.UpdatedBy
            });
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateHomePageContent([FromForm] string title, [FromForm] string description, 
            [FromForm] string? welcomeMessage, [FromForm] string? servicesOverview, 
            [FromForm] IFormFile? heroImage, [FromForm] IFormFile? bannerImage)
        {
            var content = await _context.HomePageContents.FirstOrDefaultAsync();
            
            if (content == null)
            {
                return NotFound(new { message = "Homepage content not found" });
            }

            content.Title = title;
            content.Description = description;
            content.WelcomeMessage = welcomeMessage;
            content.ServicesOverview = servicesOverview;
            content.LastUpdated = DateTime.UtcNow;
            content.UpdatedBy = User.Identity?.Name ?? "Admin";

            // Handle hero image upload
            if (heroImage != null && heroImage.Length > 0)
            {
                if (heroImage.Length > 10 * 1024 * 1024) // 10MB limit
                    return BadRequest("Hero image file size too large. Maximum size is 10MB");

                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
                if (!allowedTypes.Contains(heroImage.ContentType.ToLower()))
                    return BadRequest("Invalid hero image file type. Only JPEG, PNG, GIF, and WebP are allowed");

                using var memoryStream = new MemoryStream();
                await heroImage.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                var heroImageUpload = new ImageUpload
                {
                    FileName = heroImage.FileName,
                    ContentType = heroImage.ContentType,
                    ImageData = imageData,
                    ImageType = "hero",
                    Title = "Hero Image",
                    UploadedBy = User.Identity?.Name ?? "Admin",
                    UploadedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.ImageUploads.Add(heroImageUpload);
                await _context.SaveChangesAsync();

                content.HeroImageUrl = $"/api/image/{heroImageUpload.Id}";
            }

            // Handle banner image upload
            if (bannerImage != null && bannerImage.Length > 0)
            {
                if (bannerImage.Length > 10 * 1024 * 1024) // 10MB limit
                    return BadRequest("Banner image file size too large. Maximum size is 10MB");

                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
                if (!allowedTypes.Contains(bannerImage.ContentType.ToLower()))
                    return BadRequest("Invalid banner image file type. Only JPEG, PNG, GIF, and WebP are allowed");

                using var memoryStream = new MemoryStream();
                await bannerImage.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                var bannerImageUpload = new ImageUpload
                {
                    FileName = bannerImage.FileName,
                    ContentType = bannerImage.ContentType,
                    ImageData = imageData,
                    ImageType = "banner",
                    Title = "Banner Image",
                    UploadedBy = User.Identity?.Name ?? "Admin",
                    UploadedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.ImageUploads.Add(bannerImageUpload);
                await _context.SaveChangesAsync();

                content.BannerImageUrl = $"/api/image/{bannerImageUpload.Id}";
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Homepage content updated successfully" });
        }
    }
} 