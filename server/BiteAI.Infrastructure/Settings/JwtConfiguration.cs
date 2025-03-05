namespace BiteAI.Infrastructure.Settings;

public class JwtConfiguration
{
    public required int ExpiresInDays { get; set; }
    
    public required string Issuer { get; set; }
    
    public required string Audience { get; set; }
    
    public required string Secret { get; set; }
}