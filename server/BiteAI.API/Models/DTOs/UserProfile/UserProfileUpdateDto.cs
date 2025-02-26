using BiteAI.Infrastructure.Models;

namespace BiteAI.API.Models.DTOs.UserProfile;

public class UserProfileUpdateDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Gender Gender { get; set; }
    public int? Age { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
    public ActivityLevel ActivityLevel { get; set; }
}