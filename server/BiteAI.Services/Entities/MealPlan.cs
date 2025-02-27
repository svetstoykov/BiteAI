using BiteAI.Services.Entities.Base;
using BiteAI.Services.Entities.Enums;
using BiteAI.Services.Enums;

namespace BiteAI.Services.Entities;

public class MealPlan : BaseIdentifiableEntity
{
    public required string Name { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int DailyCalories { get; set; }
    public DietTypes DietType { get; set; }
    public DietTypeEntity? DietTypeRelation { get; set; }
    public int DurationDays { get; set; } = 7;
        
    public required string UserId { get; set; }
    
    public required ApplicationUser ApplicationUser { get; set; }
    
    public virtual ICollection<MealDay> MealDays { get; set; } = new List<MealDay>();
}
    
