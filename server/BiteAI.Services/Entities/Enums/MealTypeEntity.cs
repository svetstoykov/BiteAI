using BiteAI.Services.Entities.Base;
using BiteAI.Services.Enums;

namespace BiteAI.Services.Entities.Enums;

public class MealTypeEntity : BaseNameValueEntity<MealTypes, string>
{
    public MealTypeEntity(MealTypes value, string name) : base(value, name)
    {
    }

    public ICollection<Meal> Meals { get; set; } = new List<Meal>();
}