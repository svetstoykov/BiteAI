using BiteAI.Services.Entities;
using Microsoft.AspNetCore.Identity;

namespace BiteAI.Infrastructure.Models;

public class IdentityAccount : IdentityUser
{
    // Navigation property to ApplicationUser with 1:1 relationship
    public string ApplicationUserId { get; set; } = null!;
    public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    
    // This is duplicated in both entities to ensure we can update it without loading the full relationship
    public DateTime? LastLoginAt { get; set; }
}
