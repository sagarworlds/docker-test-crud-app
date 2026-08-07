using CrudApp.Api.Data;
using CrudApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ── Database Connection String Parser ─────────────────────────
var rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString    = FormatConnectionString(rawConnectionString);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ── CORS — allow any origin for demo; restrict in production ──
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ── Controllers & OpenAPI ─────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// ── Database initialisation on startup ───────────────────────
using (var scope = app.Services.CreateScope())
{
    var db     = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Initialising database and ensuring schema exists...");
        db.Database.EnsureCreated();

        if (!db.Products.Any())
        {
            db.Products.AddRange(
                new Product { Name = "Laptop Pro 15",          Description = "High-performance laptop for developers",              Price = 1299.99m, Category = "Electronics" },
                new Product { Name = "Wireless Headphones",    Description = "Noise-cancelling over-ear headphones",                Price =  299.99m, Category = "Electronics" },
                new Product { Name = "Ergonomic Office Chair", Description = "Comfortable mesh chair for long work sessions",       Price =  499.99m, Category = "Furniture"   },
                new Product { Name = "Standing Desk",          Description = "Electric height-adjustable standing desk",            Price =  799.99m, Category = "Furniture"   },
                new Product { Name = "USB-C Hub 7-in-1",       Description = "HDMI, USB-A × 3, SD, microSD, 100W PD pass-through", Price =   79.99m, Category = "Accessories" }
            );
            db.SaveChanges();
            logger.LogInformation("Database seeded with sample products successfully.");
        }

        logger.LogInformation("Database is ready and healthy.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database initialisation failed.");
        // Non-fatal logging so app can start even if DB takes a few seconds on cloud restart
    }
}

// ── Middleware pipeline ───────────────────────────────────────
app.UseCors("AllowAll");

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "CrudApp API";
    options.Theme = ScalarTheme.DeepSpace;
});

app.MapControllers();

app.Run();

// ── Helper: Converts postgres:// or postgresql:// URLs into Npgsql connection strings ──
static string FormatConnectionString(string? connStr)
{
    if (string.IsNullOrWhiteSpace(connStr))
        return string.Empty;

    if (connStr.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
        connStr.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
    {
        var uri      = new Uri(connStr);
        var userInfo = uri.UserInfo.Split(':');
        var username = userInfo.Length > 0 ? Uri.UnescapeDataString(userInfo[0]) : "";
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";
        var host     = uri.Host;
        var port     = uri.Port > 0 ? uri.Port : 5432;
        var database = uri.AbsolutePath.TrimStart('/');

        return $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true;";
    }

    return connStr;
}
