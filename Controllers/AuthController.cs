using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visa_consulatant.Data;
using visa_consulatant.Models;
using visa_consulatant.Models.DTOs;
using visa_consulatant.Services;

namespace visa_consulatant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            try
            {
                // Check if user exists
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == request.Username && u.IsActive);

                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                // Generate token
                var token = _jwtService.GenerateToken(user);

                return Ok(new LoginResponse
                {
                    Token = token,
                    Username = user.Username,
                    Role = user.Role,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Login failed",
                    error = ex.Message,
                    stackTrace = ex.StackTrace,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpPost("test-connection")]
        public ActionResult TestConnection()
        {
            try
            {
                var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                var pgHost = Environment.GetEnvironmentVariable("PGHOST");
                var pgPort = Environment.GetEnvironmentVariable("PGPORT");
                var pgDatabase = Environment.GetEnvironmentVariable("PGDATABASE");
                var pgUser = Environment.GetEnvironmentVariable("PGUSER");
                var pgPassword = Environment.GetEnvironmentVariable("PGPASSWORD");
                
                // Check alternative environment variables
                var railwayDbUrl = Environment.GetEnvironmentVariable("RAILWAY_DATABASE_URL");
                var dbConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
                var postgresUrl = Environment.GetEnvironmentVariable("POSTGRES_URL");
                
                var maskedDatabaseUrl = databaseUrl != null 
                    ? databaseUrl.Substring(0, Math.Min(50, databaseUrl.Length)) + "..." 
                    : "null";
                
                var maskedRailwayDbUrl = railwayDbUrl != null 
                    ? railwayDbUrl.Substring(0, Math.Min(50, railwayDbUrl.Length)) + "..." 
                    : "null";
                
                return Ok(new { 
                    databaseUrl = maskedDatabaseUrl,
                    databaseUrlLength = databaseUrl?.Length ?? 0,
                    pgHost = pgHost,
                    pgPort = pgPort,
                    pgDatabase = pgDatabase,
                    pgUser = pgUser,
                    pgPassword = pgPassword != null ? "***" : "null",
                    railwayDbUrl = maskedRailwayDbUrl,
                    dbConnectionString = dbConnectionString != null ? "***" : "null",
                    postgresUrl = postgresUrl != null ? "***" : "null",
                    jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") != null ? "set" : "null",
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpGet("ping")]
        public ActionResult Ping()
        {
            return Ok(new { 
                message = "Pong!",
                timestamp = DateTime.UtcNow,
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            });
        }

        [HttpGet("debug-jwt")]
        public ActionResult DebugJwt()
        {
            try
            {
                var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "2pHbAojhgk83cyRlNMCXntL6I05EUzFxOdYTwBVZaG9WSe4uisP7KvJrfqQmD1";
                var keyLength = jwtSecretKey.Length;
                var keyBits = keyLength * 8;
                
                return Ok(new { 
                    jwtSecretKey = jwtSecretKey != null ? "set" : "null",
                    keyLength = keyLength,
                    keyBits = keyBits,
                    keyPreview = jwtSecretKey.Length > 10 ? jwtSecretKey.Substring(0, 10) + "..." : jwtSecretKey,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message,
                    stackTrace = ex.StackTrace,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpGet("test-jwt")]
        public ActionResult TestJwt()
        {
            try
            {
                var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "2pHbAojhgk83cyRlNMCXntL6I05EUzFxOdYTwBVZaG9WSe4uisP7KvJrfqQmD1";
                var testUser = new User
                {
                    Id = 1,
                    Username = "test",
                    Email = "test@example.com",
                    Role = "Admin"
                };
                
                var token = _jwtService.GenerateToken(testUser);
                
                return Ok(new { 
                    jwtSecretKey = jwtSecretKey != null ? "set" : "null",
                    token = token,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message,
                    stackTrace = ex.StackTrace,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpPost("test-db")]
        public async Task<ActionResult> TestDatabase()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                var userCount = await _context.Users.CountAsync();
                
                return Ok(new { 
                    canConnect = canConnect,
                    userCount = userCount,
                    connectionString = _context.Database.GetConnectionString()?.Substring(0, 20) + "...",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message,
                    stackTrace = ex.StackTrace,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpPost("test-sql")]
        public async Task<ActionResult> TestSql()
        {
            try
            {
                var results = new List<object>();
                
                // Test 1: Check if we can connect
                var canConnect = await _context.Database.CanConnectAsync();
                results.Add(new { test = "Database Connection", result = canConnect ? "SUCCESS" : "FAILED" });
                
                if (!canConnect)
                {
                    return StatusCode(500, new { 
                        error = "Cannot connect to database",
                        tests = results,
                        timestamp = DateTime.UtcNow
                    });
                }
                
                // Test 2: Get database name
                var databaseName = _context.Database.GetDbConnection().Database;
                results.Add(new { test = "Database Name", result = databaseName });
                
                // Test 3: Check if Users table exists using simpler query
                var usersTableCount = await _context.Database.ExecuteSqlRawAsync(
                    "SELECT COUNT(*) FROM information_schema.tables WHERE table_name = 'Users'");
                var usersTableExists = usersTableCount > 0;
                results.Add(new { test = "Users Table Exists", result = usersTableExists ? "YES" : "NO" });
                
                // Test 4: Count users using Entity Framework
                var userCount = await _context.Users.CountAsync();
                results.Add(new { test = "User Count", result = userCount });
                
                // Test 5: Check if HomePageContents table exists
                var homePageTableCount = await _context.Database.ExecuteSqlRawAsync(
                    "SELECT COUNT(*) FROM information_schema.tables WHERE table_name = 'HomePageContents'");
                var homePageTableExists = homePageTableCount > 0;
                results.Add(new { test = "HomePageContents Table Exists", result = homePageTableExists ? "YES" : "NO" });
                
                // Test 6: Check if VisaServices table exists
                var visaServicesTableCount = await _context.Database.ExecuteSqlRawAsync(
                    "SELECT COUNT(*) FROM information_schema.tables WHERE table_name = 'VisaServices'");
                var visaServicesTableExists = visaServicesTableCount > 0;
                results.Add(new { test = "VisaServices Table Exists", result = visaServicesTableExists ? "YES" : "NO" });
                
                // Test 7: List all tables using simpler approach
                try
                {
                    var tableNames = new List<string>();
                    using var command = _context.Database.GetDbConnection().CreateCommand();
                    command.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name";
                    _context.Database.OpenConnection();
                    using var result = command.ExecuteReader();
                    while (result.Read())
                    {
                        tableNames.Add(result.GetString(0));
                    }
                    results.Add(new { test = "All Tables", result = string.Join(", ", tableNames) });
                }
                catch (Exception ex)
                {
                    results.Add(new { test = "All Tables", result = $"Error: {ex.Message}" });
                }
                
                // Test 8: Check database version using simpler approach
                try
                {
                    using var command = _context.Database.GetDbConnection().CreateCommand();
                    command.CommandText = "SELECT version()";
                    _context.Database.OpenConnection();
                    var version = command.ExecuteScalar()?.ToString();
                    results.Add(new { test = "Database Version", result = version ?? "Unknown" });
                }
                catch (Exception ex)
                {
                    results.Add(new { test = "Database Version", result = $"Error: {ex.Message}" });
                }
                
                return Ok(new { 
                    tests = results,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message,
                    stackTrace = ex.StackTrace,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpPost("run-sql")]
        public async Task<ActionResult> RunSql([FromBody] SqlRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Sql))
                {
                    return BadRequest(new { error = "SQL command is required" });
                }
                
                // Only allow SELECT queries for security
                var sql = request.Sql.Trim();
                if (!sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { error = "Only SELECT queries are allowed for security" });
                }
                
                var result = await _context.Database.SqlQueryRaw<object>(sql).ToListAsync();
                
                return Ok(new { 
                    sql = sql,
                    result = result,
                    count = result.Count,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message,
                    sql = request.Sql,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest request)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                {
                    return BadRequest(new { message = "Username already exists" });
                }

                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                {
                    return BadRequest(new { message = "Email already exists" });
                }

                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Role = "User",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    message = "Failed to register user",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
} 