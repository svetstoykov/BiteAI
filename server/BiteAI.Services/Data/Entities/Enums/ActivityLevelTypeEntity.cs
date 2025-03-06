using BiteAI.Services.Contracts.Enums;
using BiteAI.Services.Data.Entities.Base;

namespace BiteAI.Services.Data.Entities.Enums;

public class ActivityLevelTypeEntity : BaseNameValueEntity<ActivityLevels, string>
{
    public ActivityLevelTypeEntity(ActivityLevels value, string name) : base(value, name)
    {
    }

    public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
}