using BiteAI.Services.Enums;

namespace BiteAI.Services.Contracts.UserProfile;

public class UserProfileDto
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public Genders Gender { get; set; }
    public int? Age { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
    public ActivityLevels ActivityLevels { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}