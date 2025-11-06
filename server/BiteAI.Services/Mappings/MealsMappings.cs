using BiteAI.Services.Contracts.Meals;
using BiteAI.Services.Data.Entities;

namespace BiteAI.Services.Mappings;

public static class MealsMappings
{
    public static MealDay ToMealDayEntity(this MealDayDto mealDayDto)
        => new()
        {
            DayNumber = mealDayDto.DayNumber,
            TotalCalories = mealDayDto.TotalCalories > 0 ? mealDayDto.TotalCalories : mealDayDto.DailyMeals.Sum(m => m.Calories),
            Meals = mealDayDto.DailyMeals.Select(m => m.ToMealEntity()).ToList()
        };

    public static MealPlanDto ToMealPlanDto(this MealPlan mealPlan)
        => new()
        {
            Id = mealPlan.Id.ToString(),
            CreatedDate = mealPlan.CreatedDate,
            DailyCalories = mealPlan.DailyCalories,
            DietType = mealPlan.DietType,
            DurationDays = mealPlan.DurationDays,
            MealDays = mealPlan.MealDays
                .OrderBy(md => md.DayNumber)
                .Select(md => md.ToMealDayDto())
                .ToList()
        };

    public static Meal ToMealEntity(this MealDto mealDto)
        => new()
        {
            Name = mealDto.Name,
            Recipe = mealDto.Recipe,
            CarbsInGrams = mealDto.CarbsInGrams,
            FatInGrams = mealDto.FatInGrams,
            ProteinInGrams = mealDto.ProteinInGrams,
            Calories = mealDto.Calories,
            MealType = mealDto.MealType,
            MealOrder = mealDto.MealOrder
        };

    public static MealDto ToMealDto(this Meal meal)
        => new()
        {
            Name = meal.Name,
            Recipe = meal.Recipe,
            CarbsInGrams = meal.CarbsInGrams,
            FatInGrams = meal.FatInGrams,
            ProteinInGrams = meal.ProteinInGrams,
            Calories = meal.Calories,
            MealType = meal.MealType,
            MealOrder = meal.MealOrder
        };

    public static MealDayDto ToMealDayDto(this MealDay mealDay)
        => new()
        {
            DayNumber = mealDay.DayNumber,
            TotalCalories = mealDay.TotalCalories,
            DailyMeals = mealDay.Meals
                .OrderBy(m => m.MealOrder)
                .ThenBy(m => m.MealType)
                .Select(m => m.ToMealDto())
                .ToList()
        };
}