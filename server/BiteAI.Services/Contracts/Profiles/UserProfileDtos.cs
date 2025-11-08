using System.ComponentModel.DataAnnotations;
using BiteAI.Services.Contracts.Enums;

namespace BiteAI.Services.Contracts.Profiles;

public class UserProfileDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public Genders? Gender { get; set; }
    public int? Age { get; set; }
    public double? WeightInKg { get; set; }
    public double? HeightInCm { get; set; }
    public ActivityLevels? ActivityLevel { get; set; }
    public DietType? PreferredDietType { get; set; }
    public List<Allergies> Allergies { get; set; } = new();
    public List<FoodDislikes> FoodDislikes { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsProfileComplete { get; set; }
    public DateTime? OnboardingCompletedAt { get; set; }
}

public class CreateUserProfileDto
{
    [Required]
    public Genders Gender { get; set; }

    [Required]
    [Range(13, 120)]
    public int Age { get; set; }

    [Required]
    [Range(30, 300)]
    public double WeightInKg { get; set; }

    [Required]
    [Range(100, 250)]
    public double HeightInCm { get; set; }

    [Required]
    public ActivityLevels ActivityLevel { get; set; }

    public DietType? PreferredDietType { get; set; }
    public List<Allergies>? Allergies { get; set; }
    public List<FoodDislikes>? FoodDislikes { get; set; }
}

public class UpdateUserProfileDto
{
    public Genders? Gender { get; set; }

    [Range(13, 120)]
    public int? Age { get; set; }

    [Range(30, 300)]
    public double? WeightInKg { get; set; }

    [Range(100, 250)]
    public double? HeightInCm { get; set; }

    public ActivityLevels? ActivityLevel { get; set; }
    public DietType? PreferredDietType { get; set; }
    public List<Allergies>? Allergies { get; set; }
    public List<FoodDislikes>? FoodDislikes { get; set; }
}
