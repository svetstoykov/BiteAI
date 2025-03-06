using BiteAI.Services.Contracts.Enums;
using BiteAI.Services.Data.Entities.Base;

namespace BiteAI.Services.Data.Entities.Enums;

public class MealTypeEntity : BaseNameValueEntity<MealTypes, string>
{
    public MealTypeEntity(MealTypes value, string name) : base(value, name)
    {
    }

    public ICollection<Meal> Meals { get; set; } = new List<Meal>();
}