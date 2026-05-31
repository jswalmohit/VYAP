using System.Reflection;
using System.Data;
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
        bool dbConnected = false;
        string? dbError = null;
        try
        {
            dbConnected = await _context.Database.CanConnectAsync();

            // If CanConnectAsync returns false without exception, try opening a raw connection
            if (!dbConnected)
            {
                try
                {
                    var conn = _context.Database.GetDbConnection();
                    await conn.OpenAsync();
                    dbConnected = conn.State == ConnectionState.Open;
                    await conn.CloseAsync();
                }
                catch (Exception openEx)
                {
                    _logger.LogError(openEx, "Database open attempt failed");
                    dbError = openEx.Message + (openEx.InnerException != null ? " | " + openEx.InnerException.Message : string.Empty);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database CanConnectAsync failed");
            dbConnected = false;
            dbError = ex.Message + (ex.InnerException != null ? " | " + ex.InnerException.Message : string.Empty);
        }

        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                      ?? assembly.GetName().Version?.ToString()
                      ?? "unknown";

        return Ok(new { DatabaseConnected = dbConnected, DatabaseError = dbError, Version = version });
    }
}
