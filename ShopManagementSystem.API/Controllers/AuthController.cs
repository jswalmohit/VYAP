using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ShopManagementSystem.API.DTOs;
using ShopManagementSystem.API.Models;

namespace ShopManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    private readonly IConfiguration _configuration;

    public AuthController(JwtSettings jwtSettings, IConfiguration configuration)
    {
        _jwtSettings = jwtSettings;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        // Validate credentials - using config for demo. Change to your user store.
        var cfgUser = _configuration["Auth:Username"] ?? "admin";
        var cfgPass = _configuration["Auth:Password"] ?? "password";

        if (request.Username != cfgUser || request.Password != cfgPass)
        {
            return Unauthorized(new { Message = "Invalid username or password." });
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, request.Username),
            new(ClaimTypes.NameIdentifier, request.Username)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new LoginResponse { Token = tokenString, ExpiresAt = tokenDescriptor.Expires!.Value });
    }
}
