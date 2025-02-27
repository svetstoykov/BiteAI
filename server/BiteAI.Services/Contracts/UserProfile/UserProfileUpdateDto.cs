using BiteAI.Services.Enums;

namespace BiteAI.Services.Contracts.UserProfile;

public class UserProfileUpdateDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Genders Gender { get; set; }
    public int Age { get; set; }
    public double WeightInKg { get; set; }
    public double HeightInCm { get; set; }
    public ActivityLevels ActivityLevels { get; set; }
}