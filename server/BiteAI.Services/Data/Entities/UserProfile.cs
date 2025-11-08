using BiteAI.Services.Contracts.Enums;
using BiteAI.Services.Data.Entities.Enums;

namespace BiteAI.Services.Data.Entities;

public class UserProfile
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    // Physical Attributes
    public Genders? Gender { get; set; }
    public GenderTypeEntity? GenderRelation { get; set; }
    public int? Age { get; set; }
    public double? WeightInKg { get; set; }
    public double? HeightInCm { get; set; }

    // Activity
    public ActivityLevels? ActivityLevel { get; set; }
    public ActivityLevelTypeEntity? ActivityLevelRelation { get; set; }

    // Preferences
    public DietType? PreferredDietType { get; set; }
    public DietTypeEntity? PreferredDietTypeRelation { get; set; }

    // Dietary (many-to-many with enum entities)
    public ICollection<AllergyTypeEntity> Allergies { get; set; } = new List<AllergyTypeEntity>();
    public ICollection<FoodDislikeTypeEntity> FoodDislikes { get; set; } = new List<FoodDislikeTypeEntity>();

    // Metadata
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsProfileComplete { get; set; }
    public DateTime? OnboardingCompletedAt { get; set; }
}
