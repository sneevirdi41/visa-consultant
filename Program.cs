using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using visa_consulatant.Data;
using visa_consulatant.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework with PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Try to get DATABASE_URL from environment variables
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
var pgHost = Environment.GetEnvironmentVariable("PGHOST");
var pgPort = Environment.GetEnvironmentVariable("PGPORT");
var pgDatabase = Environment.GetEnvironmentVariable("PGDATABASE");
var pgUser = Environment.GetEnvironmentVariable("PGUSER");
var pgPassword = Environment.GetEnvironmentVariable("PGPASSWORD");

Console.WriteLine($"DATABASE_URL: {(string.IsNullOrEmpty(databaseUrl) ? "null" : "set")}");
Console.WriteLine($"PGHOST: {pgHost}");
Console.WriteLine($"PGPORT: {pgPort}");
Console.WriteLine($"PGDATABASE: {pgDatabase}");
Console.WriteLine($"PGUSER: {pgUser}");
Console.WriteLine($"PGPASSWORD: {(string.IsNullOrEmpty(pgPassword) ? "null" : "set")}");

// Check if DATABASE_URL contains Railway variable reference format
if (!string.IsNullOrEmpty(databaseUrl) && databaseUrl.StartsWith("${{"))
{
    Console.WriteLine("WARNING: DATABASE_URL contains Railway variable reference format. This should be resolved by Railway.");
    Console.WriteLine($"Raw DATABASE_URL: {databaseUrl}");
    // Railway should resolve this automatically, but if not, we'll fall back to individual variables
}

// Build connection string from individual environment variables if DATABASE_URL is not available or is a reference
// BUT only if we're not in development mode and explicitly want to use local SQLite
if (string.IsNullOrEmpty(connectionString) && (string.IsNullOrEmpty(databaseUrl) || databaseUrl.StartsWith("${{")))
{
    // Check if we're running locally and want to force SQLite
    var forceLocalSqlite = Environment.GetEnvironmentVariable("FORCE_LOCAL_SQLITE");
    var isLocalDevelopment = builder.Environment.IsDevelopment() || !string.IsNullOrEmpty(forceLocalSqlite);
    
    if (isLocalDevelopment)
    {
        // Force SQLite for local development
        connectionString = "Data Source=GuruKirpaVisaConsultancy.db";
        Console.WriteLine("Forcing SQLite for local development");
    }
    else if (!string.IsNullOrEmpty(pgHost) && !string.IsNullOrEmpty(pgDatabase) && !string.IsNullOrEmpty(pgUser) && !string.IsNullOrEmpty(pgPassword))
    {
        var port = string.IsNullOrEmpty(pgPort) ? "5432" : pgPort;
        connectionString = $"Host={pgHost};Port={port};Database={pgDatabase};Username={pgUser};Password={pgPassword};SSL Mode=Prefer;Trust Server Certificate=true;";
        Console.WriteLine("Built connection string from individual environment variables");
    }
    else
    {
        Console.WriteLine("WARNING: No database connection string found!");
        // Use SQLite for local development
        connectionString = "Data Source=GuruKirpaVisaConsultancy.db";
        Console.WriteLine("Using SQLite for local development");
    }
}
else if (!string.IsNullOrEmpty(databaseUrl) && !databaseUrl.StartsWith("${{"))
{
    connectionString = databaseUrl;
    Console.WriteLine("Using DATABASE_URL from environment");
}

// Force build connection string from individual variables if we're in production and have the variables
if (builder.Environment.IsProduction() && string.IsNullOrEmpty(connectionString))
{
    if (!string.IsNullOrEmpty(pgHost) && !string.IsNullOrEmpty(pgDatabase) && !string.IsNullOrEmpty(pgUser) && !string.IsNullOrEmpty(pgPassword))
    {
        var port = string.IsNullOrEmpty(pgPort) ? "5432" : pgPort;
        connectionString = $"Host={pgHost};Port={port};Database={pgDatabase};Username={pgUser};Password={pgPassword};SSL Mode=Prefer;Trust Server Certificate=true;";
        Console.WriteLine("Forced connection string build from individual environment variables for production");
    }
}

// Additional fallback: Try to get the actual DATABASE_URL from Railway's internal resolution
if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("visa_consultant.db"))
{
    // Try alternative environment variable names that Railway might use
    var alternativeDbUrl = Environment.GetEnvironmentVariable("RAILWAY_DATABASE_URL") 
        ?? Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")
        ?? Environment.GetEnvironmentVariable("POSTGRES_URL");
    
    if (!string.IsNullOrEmpty(alternativeDbUrl))
    {
        connectionString = alternativeDbUrl;
        Console.WriteLine($"Using alternative database URL: {alternativeDbUrl.Substring(0, Math.Min(30, alternativeDbUrl.Length))}...");
    }
}

// Log connection string for debugging (masked)
var maskedConnectionString = connectionString != null 
    ? connectionString.Substring(0, Math.Min(20, connectionString.Length)) + "..." 
    : "null";
Console.WriteLine($"Final connection string: {maskedConnectionString}");

// Use SQLite for local development, PostgreSQL for production
var forceLocalSqliteProvider = Environment.GetEnvironmentVariable("FORCE_LOCAL_SQLITE");
var isLocalDevelopmentProvider = builder.Environment.IsDevelopment() || !string.IsNullOrEmpty(forceLocalSqliteProvider);

if (isLocalDevelopmentProvider && (connectionString.Contains("Data Source=") || connectionString.Contains("GuruKirpaVisaConsultancy.db")))
{
    Console.WriteLine("Configuring SQLite for local development...");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));
}
else
{
    Console.WriteLine("Configuring PostgreSQL for production...");
    if (connectionString.StartsWith("postgresql://") || connectionString.StartsWith("postgres://"))
    {
        Console.WriteLine("Converting Railway DATABASE_URL format...");
        Console.WriteLine($"Original connection string: {connectionString}");
        try
        {
            // Manual parsing of postgresql:// format
            var uri = connectionString.Replace("postgresql://", "").Replace("postgres://", "");
            var atIndex = uri.IndexOf('@');
            var slashIndex = uri.IndexOf('/', atIndex);
            
            var userInfo = uri.Substring(0, atIndex);
            var hostPort = uri.Substring(atIndex + 1, slashIndex - atIndex - 1);
            var database = uri.Substring(slashIndex + 1);
            
            var colonIndex = userInfo.IndexOf(':');
            var username = userInfo.Substring(0, colonIndex);
            var password = userInfo.Substring(colonIndex + 1);
            
            var hostPortParts = hostPort.Split(':');
            var host = hostPortParts[0];
            var port = hostPortParts.Length > 1 ? hostPortParts[1] : "5432";
            
            Console.WriteLine($"Extracted - Username: {username}, Host: {host}, Port: {port}, Database: {database}");
            
            // Build the connection string in the correct format for Npgsql
            connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Prefer;Trust Server Certificate=true;";
            Console.WriteLine($"Converted connection string: {connectionString.Substring(0, Math.Min(30, connectionString.Length))}...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing DATABASE_URL: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            // Fall back to using the original connection string
        }
    }
    
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));
}

// Configure JWT Authentication
var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "YourSuperSecretJWTKey2024!";
Console.WriteLine($"JWT Secret Key configured: {!string.IsNullOrEmpty(jwtSecretKey)}");

var key = Encoding.ASCII.GetBytes(jwtSecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

// Add Authorization
builder.Services.AddAuthorization();

// Register Services
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Visa Consultant API", Version = "v1" });
    
    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Enable Swagger in all environments for Railway health checks
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseStaticFiles(); // Added to serve static files

// Configure default files to serve index.html at root URL
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" }
});

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check endpoint is now handled by HealthController

// Ensure database is migrated with retry logic
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        logger.LogInformation("Starting database migration...");
        logger.LogInformation($"Connection string: {connectionString?.Substring(0, Math.Min(50, connectionString.Length))}...");
        
        // Test database connection first
        logger.LogInformation("Testing database connection...");
        var canConnect = await context.Database.CanConnectAsync();
        logger.LogInformation($"Database connection test: {(canConnect ? "SUCCESS" : "FAILED")}");
        
        if (!canConnect)
        {
            logger.LogError("Cannot connect to database. Application will start but database operations will fail.");
            logger.LogError("This might be due to database not being ready yet. Will retry on first request.");
            // Don't throw - let the app start even if database is not available
        }
        else
        {
            try
            {
                // Ensure database exists
                logger.LogInformation("Ensuring database exists...");
                context.Database.EnsureCreated();
                logger.LogInformation("Database created/verified successfully.");
                
                // Apply migrations
                logger.LogInformation("Applying database migrations...");
                context.Database.Migrate();
                logger.LogInformation("Database migrated successfully.");
            }
            catch (Exception migrationEx)
            {
                logger.LogError(migrationEx, "Failed to migrate database. Application will continue but database operations may fail.");
                Console.WriteLine($"Database migration error: {migrationEx.Message}");
                // Don't throw - let the app start even if migration fails
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to connect to database during startup. Application will continue but database operations may fail.");
        Console.WriteLine($"Database connection error during startup: {ex.Message}");
        // Don't throw - let the app start even if connection fails
    }
}

app.Run();
