using BiteAI.Services.Enums;

namespace BiteAI.Services.Entities;

public class ApplicationUser
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public Gender Gender { get; set; }
    public int? Age { get; set; }
    public double? Weight { get; set; } // in kg
    public double? Height { get; set; } // in cm
    public ActivityLevel ActivityLevel { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
}