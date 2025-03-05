using BiteAI.Services.Entities.Base;
using BiteAI.Services.Entities.Enums;
using BiteAI.Services.Enums;

namespace BiteAI.Services.Entities;

public class MealPlan : BaseIdentifiableEntity
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int DailyCalories { get; set; }
    public DietTypes DietType { get; set; }
    public DietTypeEntity? DietTypeRelation { get; set; }
    public int DurationDays { get; set; } = 7;
        
    public string ApplicationUserId { get; set; }
    
    public ApplicationUser ApplicationUser { get; set; }
    
    public virtual ICollection<MealDay> MealDays { get; set; } = new List<MealDay>();
    
    public DateTime? LastLoginAt { get; set; }
}
    
