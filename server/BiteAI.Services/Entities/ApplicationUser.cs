using BiteAI.Services.Entities.Base;
using BiteAI.Services.Entities.Enums;
using BiteAI.Services.Enums;

namespace BiteAI.Services.Entities;

public class ApplicationUser : BaseIdentifiableEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public Genders? Gender { get; set; }
    public GenderTypeEntity? GenderRelation { get; set; }
    public int? Age { get; set; }
    public double? WeightInKg { get; set; }
    public double? HeightInCm { get; set; }
    public ActivityLevels? ActivityLevels { get; set; }
    
    public ActivityLevelTypeEntity? ActivityLevelRelation { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
}