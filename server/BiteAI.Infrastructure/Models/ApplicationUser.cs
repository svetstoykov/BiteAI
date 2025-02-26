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

public enum Gender
{
    Male = 1,
    Female = 2,
}

public enum ActivityLevel
{
    NotSpecified = 0,
    Sedentary = 1,     // Little to no exercise
    LightlyActive = 2, // Light exercise 1-3 days/week
    ModeratelyActive = 3, // Moderate exercise 3-5 days/week
    VeryActive = 4,    // Hard exercise 6-7 days/week
    ExtraActive = 5    // Very hard exercise & physical job or 2x training
}