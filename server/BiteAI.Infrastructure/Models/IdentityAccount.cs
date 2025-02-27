using BiteAI.Services.Entities;
using Microsoft.AspNetCore.Identity;

namespace BiteAI.Infrastructure.Models;

public class IdentityAccount : IdentityUser
{
    public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    public DateTime? LastLoginAt { get; set; }
}
