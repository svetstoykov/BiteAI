using Microsoft.AspNetCore.Identity;

namespace BiteAI.Services.Data.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
    
    public DateTime? LastLoginAt { get; set; }

    // Navigation to new 1:1 UserProfile
    public virtual UserProfile? Profile { get; set; }
}