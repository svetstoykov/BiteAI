using BiteAI.Services.Entities;
using BiteAI.Services.Enums;
using BiteAI.Services.Models;

namespace BiteAI.Services.Mappings;

public static class MealDayMapping
{
    public static MealDay ToMealDayEntity(this MealDayDto mealDayDto)
    {
        return new MealDay
        {
            DayNumber = mealDayDto.DayNumber,
            TotalCalories = mealDayDto.DailyMeals.Sum(m => m.Calories),
            Meals = mealDayDto.DailyMeals.Select(m => new Meal
                {
                    Name = m.Name,
                    Recipe = m.Recipe,
                    CarbsInGrams = m.CarbsInGrams,
                    FatInGrams = m.FatInGrams,
                    ProteinInGrams = m.ProteinInGrams,
                    Calories = m.Calories,
                    MealType = m.MealType
                })
                .ToList()
        };
    }
}