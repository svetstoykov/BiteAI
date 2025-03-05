using BiteAI.Services.Entities.Enums;
using BiteAI.Services.Enums;
using Microsoft.AspNetCore.Identity;

namespace BiteAI.Services.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Genders? Gender { get; set; }
    public GenderTypeEntity? GenderRelation { get; set; }
    public int? Age { get; set; }
    public double? WeightInKg { get; set; }
    public double? HeightInCm { get; set; }
    public ActivityLevels? ActivityLevels { get; set; }
    
    public ActivityLevelTypeEntity? ActivityLevelRelation { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
    
    public DateTime? LastLoginAt { get; set; }
}