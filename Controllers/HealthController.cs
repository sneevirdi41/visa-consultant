using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visa_consulatant.Data;

namespace visa_consulatant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HealthController> _logger;

        public HealthController(ApplicationDbContext context, ILogger<HealthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Check database connection
                var canConnect = await _context.Database.CanConnectAsync();
                
                var healthStatus = new
                {
                    status = "healthy",
                    timestamp = DateTime.UtcNow,
                    database = canConnect ? "connected" : "disconnected",
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                    version = "1.0.0"
                };

                if (!canConnect)
                {
                    _logger.LogWarning("Database connection failed");
                    return StatusCode(503, new { status = "unhealthy", database = "disconnected" });
                }

                return Ok(healthStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(503, new { status = "unhealthy", error = ex.Message });
            }
        }

        [HttpGet("db")]
        public async Task<IActionResult> DatabaseHealth()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
                
                return Ok(new
                {
                    connected = canConnect,
                    pendingMigrations = pendingMigrations.ToArray(),
                    databaseName = _context.Database.GetDbConnection().Database
                });
            }
            catch (Exception ex)
            {
                return StatusCode(503, new { error = ex.Message });
            }
        }
    }
} 