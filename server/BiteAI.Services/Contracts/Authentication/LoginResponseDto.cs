namespace BiteAI.Services.Contracts.Authentication;

public class LoginResponseDto
{
    public required string Token { get; set; }
    public string? RefreshToken { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public DateTime TokenExpiration { get; set; }
    public Guid UserId { get; set; }
}