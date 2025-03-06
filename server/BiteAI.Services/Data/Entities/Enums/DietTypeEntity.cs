using BiteAI.Services.Contracts.Enums;
using BiteAI.Services.Data.Entities.Base;

namespace BiteAI.Services.Data.Entities.Enums;

public class DietTypeEntity : BaseNameValueEntity<DietTypes, string>
{
    public DietTypeEntity(DietTypes value, string name) : base(value, name)
    {
    }

    public ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
}