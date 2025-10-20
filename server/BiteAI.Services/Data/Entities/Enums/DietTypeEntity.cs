using BiteAI.Services.Contracts.Enums;
using BiteAI.Services.Data.Entities.Base;

namespace BiteAI.Services.Data.Entities.Enums;

public class DietTypeEntity : BaseNameValueEntity<DietType, string>
{
    public DietTypeEntity(DietType value, string name) : base(value, name)
    {
    }

    public ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
}