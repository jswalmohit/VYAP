using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Infrastructure.Persistence;

namespace ShopManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusCheckController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<StatusCheckController> _logger;

    public StatusCheckController(AppDbContext context, ILogger<StatusCheckController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        bool dbConnected;
        string? dbError = null;
        try
        {
            dbConnected = await _context.Database.CanConnectAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database connection failed");
            dbConnected = false;
            dbError = ex.Message;
        }

        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                      ?? assembly.GetName().Version?.ToString()
                      ?? "unknown";

        return Ok(new { DatabaseConnected = dbConnected, DatabaseError = dbError, Version = version });
    }
}
