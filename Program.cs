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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? Environment.GetEnvironmentVariable("DATABASE_URL");

// Log connection string for debugging (masked)
var maskedConnectionString = connectionString != null 
    ? connectionString.Substring(0, Math.Min(20, connectionString.Length)) + "..." 
    : "null";
Console.WriteLine($"Connection string: {maskedConnectionString}");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("WARNING: No database connection string found!");
    // Use a fallback connection string for development
    connectionString = "Host=localhost;Database=visa_consultant;Username=postgres;Password=password";
}
else if (connectionString.StartsWith("postgresql://") || connectionString.StartsWith("postgres://"))
{
    Console.WriteLine("Converting Railway DATABASE_URL format...");
    Console.WriteLine($"Original connection string: {connectionString}");
    try
    {
        // Handle URL encoding in password
        var uriString = connectionString.Replace("postgresql://", "http://").Replace("postgres://", "http://");
        Console.WriteLine($"URI string: {uriString}");
        
        var uri = new Uri(uriString);
        Console.WriteLine($"Parsed URI - Host: {uri.Host}, Port: {uri.Port}, Path: {uri.AbsolutePath}");
        
        var userInfo = uri.UserInfo.Split(':');
        var username = Uri.UnescapeDataString(userInfo[0]);
        var password = Uri.UnescapeDataString(userInfo[1]);
        var host = uri.Host;
        var port = uri.Port;
        var database = uri.AbsolutePath.TrimStart('/');
        
        Console.WriteLine($"Extracted - Username: {username}, Host: {host}, Port: {port}, Database: {database}");
        
        connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};";
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

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Add a simple health check endpoint
app.MapGet("/health", () => new { status = "healthy", timestamp = DateTime.UtcNow });

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
            // Don't throw - let the app start even if database is not available
        }
        else
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
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to migrate database. Application will continue but database operations may fail.");
        Console.WriteLine($"Database migration error: {ex.Message}");
        // Don't throw - let the app start even if migration fails
    }
}

app.Run();
