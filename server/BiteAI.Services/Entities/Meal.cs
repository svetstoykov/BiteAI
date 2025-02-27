using BiteAI.Services.Entities.Base;
using BiteAI.Services.Entities.Enums;
using BiteAI.Services.Enums;

namespace BiteAI.Services.Entities;

public class Meal : BaseIdentifiableEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Recipe { get; set; }
    public int Calories { get; set; }
    public MealTypes MealType { get; set; }
    public MealTypeEntity? MealTypeRelation { get; set; }
    
    public int ProteinInGrams { get; set; } 
    public int CarbsInGrams { get; set; } 
    public int FatInGrams { get; set; }

    public string MealDayId { get; set; } = null!;
    public virtual MealDay? MealDay { get; set; }
}
    