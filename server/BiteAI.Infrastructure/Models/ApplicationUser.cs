using BiteAI.Services.Entities;
using BiteAI.Services.Enums;
using Microsoft.AspNetCore.Identity;

namespace BiteAI.Infrastructure.Models;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Gender Gender { get; set; }
    public int? Age { get; set; }
    public double? Weight { get; set; } // in kg
    public double? Height { get; set; } // in cm
    public ActivityLevel ActivityLevel { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
        
    // Navigation properties
    public virtual ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
}
