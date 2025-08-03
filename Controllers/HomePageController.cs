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
        public async Task<ActionResult> UpdateHomePageContent(UpdateHomePageContentDto request)
        {
            var content = await _context.HomePageContents.FirstOrDefaultAsync();
            
            if (content == null)
            {
                return NotFound(new { message = "Homepage content not found" });
            }

            content.Title = request.Title;
            content.Description = request.Description;
            content.HeroImageUrl = request.HeroImageUrl;
            content.LogoUrl = request.LogoUrl;
            content.BannerImageUrl = request.BannerImageUrl;
            content.WelcomeMessage = request.WelcomeMessage;
            content.ServicesOverview = request.ServicesOverview;
            content.LastUpdated = DateTime.UtcNow;
            content.UpdatedBy = User.Identity?.Name ?? "Admin";

            await _context.SaveChangesAsync();

            return Ok(new { message = "Homepage content updated successfully" });
        }
    }
} 