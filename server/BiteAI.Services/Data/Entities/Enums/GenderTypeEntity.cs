using BiteAI.Services.Contracts.Enums;
using BiteAI.Services.Data.Entities.Base;

namespace BiteAI.Services.Data.Entities.Enums;

public class GenderTypeEntity : BaseNameValueEntity<Genders, string>
{
    public GenderTypeEntity(Genders value, string name) : base(value, name)
    {
    }

    public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
}