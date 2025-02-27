using BiteAI.Services.Entities.Base;
using BiteAI.Services.Enums;

namespace BiteAI.Services.Entities.Enums;

public class DietTypeEntity : BaseNameValueEntity<DietTypes, string>
{
    public DietTypeEntity(DietTypes value, string name) : base(value, name)
    {
    }

    public ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
}