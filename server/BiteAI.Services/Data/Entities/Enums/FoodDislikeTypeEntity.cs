using BiteAI.Services.Contracts.Enums;
using BiteAI.Services.Data.Entities.Base;

namespace BiteAI.Services.Data.Entities.Enums;

public class FoodDislikeTypeEntity : BaseNameValueEntity<FoodDislikes, string>
{
    public FoodDislikeTypeEntity(FoodDislikes value, string name) : base(value, name)
    {
    }

    public ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
}
