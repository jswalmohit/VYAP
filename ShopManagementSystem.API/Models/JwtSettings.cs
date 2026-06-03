namespace ShopManagementSystem.API.Models;

public class JwtSettings
{
    public string Key { get; set; } = "super_secret_development_key_please_change";
    public string Issuer { get; set; } = "VyapSetu";
    public string Audience { get; set; } = "VyapSetuClients";
    public int ExpiryMinutes { get; set; } = 60;
}
