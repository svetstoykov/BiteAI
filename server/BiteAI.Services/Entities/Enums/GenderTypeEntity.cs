using BiteAI.Services.Entities.Base;
using BiteAI.Services.Enums;

namespace BiteAI.Services.Entities.Enums;

public class GenderTypeEntity : BaseNameValueEntity<Genders, string>
{
    public GenderTypeEntity(Genders value, string name) : base(value, name)
    {
    }

    public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
}