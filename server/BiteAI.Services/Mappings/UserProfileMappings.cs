using BiteAI.Services.Contracts.Profiles;
using BiteAI.Services.Data.Entities;
using System.Linq;

namespace BiteAI.Services.Mappings;

internal static class UserProfileMappings
{
    public static UserProfileDto ToDto(this UserProfile p) => new()
    {
        Id = p.Id,
        UserId = p.UserId,
        Gender = p.Gender,
        Age = p.Age,
        WeightInKg = p.WeightInKg,
        HeightInCm = p.HeightInCm,
        ActivityLevel = p.ActivityLevel,
        PreferredDietType = p.PreferredDietType,
        Allergies = p.Allergies.Select(a => a.Value).ToList(),
        FoodDislikes = p.FoodDislikes.Select(fd => fd.Value).ToList(),
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt,
        IsProfileComplete = p.IsProfileComplete,
        OnboardingCompletedAt = p.OnboardingCompletedAt
    };
}