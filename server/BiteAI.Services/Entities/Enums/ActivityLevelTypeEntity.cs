using BiteAI.Services.Entities.Base;
using BiteAI.Services.Enums;

namespace BiteAI.Services.Entities.Enums;

public class ActivityLevelTypeEntity : BaseNameValueEntity<ActivityLevels, string>
{
    public ActivityLevelTypeEntity(ActivityLevels value, string name) : base(value, name)
    {
    }

    public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
}