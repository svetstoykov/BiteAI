using BiteAI.Services.Contracts.Enums;
using BiteAI.Services.Data.Entities.Base;
using BiteAI.Services.Data.Entities.Enums;

namespace BiteAI.Services.Data.Entities;

public class Meal : BaseIdentifiableEntity
{
    public required string Name { get; set; } 
    public required string Recipe { get; set; }
    public int Calories { get; set; }
    public MealTypes MealType { get; set; }
    public MealTypeEntity? MealTypeRelation { get; set; }
    
    public int ProteinInGrams { get; set; } 
    public int CarbsInGrams { get; set; } 
    public int FatInGrams { get; set; }

    public Guid MealDayId { get; set; }
    public virtual MealDay? MealDay { get; set; }
}
    