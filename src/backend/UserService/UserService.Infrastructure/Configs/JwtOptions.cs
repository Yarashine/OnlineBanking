namespace UserService.Infrastructure.Configs;

public class JwtOptions
{
    public string PrivateKeyPath { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public int ExpiryAccessInMinutes { get; set; }
    public int ExpiryRefreshInDays { get; set; }
}
