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

    public StatusCheckController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        bool dbConnected;
        try
        {
            dbConnected = await _context.Database.CanConnectAsync();
        }
        catch
        {
            dbConnected = false;
        }

        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                      ?? assembly.GetName().Version?.ToString()
                      ?? "unknown";

        return Ok(new { DatabaseConnected = dbConnected, Version = version });
    }
}
