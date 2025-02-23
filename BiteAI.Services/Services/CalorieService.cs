using BiteAI.Services.Contracts;
using BiteAI.Services.Interfaces;
using BiteAI.Services.Validation.Errors;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Services;

public class CalorieService : ICalorieService
{
    // Constants
    private const double MinWeightKg = 40;
    private const double MaxWeightKg = 250;
    private const double MinHeightCm = 130;
    private const double MaxHeightCm = 250;
    private const int MinAge = 18;
    private const int MaxAge = 100;
    private const int MinExerciseDays = 0;
    private const int MaxExerciseDays = 7;
    private const double MaxWeeklyWeightChangeKg = 1.0;
    private const int MinTargetWeeks = 4;
    private const int CaloriesPerKg = 7700; 

    public Result<CaloricIntakeForWeightGoalsDto> CalculateCalorieGoals(MetabolicProfileDto metabolicProfile)
    {
        var validationResult = this.ValidateMetabolicProfile(metabolicProfile);
        if (validationResult.IsFailure)
            return Result.ErrorFromResult<CaloricIntakeForWeightGoalsDto>(validationResult);

        var bmr = CalculateBmr(metabolicProfile);
        var tdee = CalculateTdee(bmr, metabolicProfile.ExerciseDaysPerWeek);

        var goalCalories = new CaloricIntakeForWeightGoalsDto
        {
            MaintainCalories = tdee,
            LoseOneKgCalories = tdee - (CaloriesPerKg / 7),
            LoseHalfKgCalories = tdee - (CaloriesPerKg / 14),
            GainHalfKgCalories = tdee + (CaloriesPerKg / 14),
            GainOneKgCalories = tdee + (CaloriesPerKg / 7)
        };

        // Ensure minimum safe calories
        var minSafeCalories = metabolicProfile.IsMale ? 1500 : 1200;
        goalCalories.LoseOneKgCalories = Math.Max(goalCalories.LoseOneKgCalories, minSafeCalories);
        goalCalories.LoseHalfKgCalories = Math.Max(goalCalories.LoseHalfKgCalories, minSafeCalories);

        return Result.Success(goalCalories);
    }

    public Result<int> CalculateTargetDailyCalories(WeightGoalPlanDto weightPlan)
    {
        var metabolicProfile = weightPlan.MetabolicProfile;
        
        var validationResult = this.ValidateGoalRequest(weightPlan);
        if (validationResult.IsFailure)
            return Result.ErrorFromResult<int>(validationResult);

        var totalWeightChange = weightPlan.TargetWeightKg - metabolicProfile.WeightKg;
        var weeklyWeightChange = totalWeightChange / weightPlan.TargetWeeks;

        if (Math.Abs(weeklyWeightChange) > MaxWeeklyWeightChangeKg)
        {
            return Result.Fail<int>(Error.Validation(
                $"Weight change of {Math.Abs(weeklyWeightChange):F1} kg per week is not realistic. " +
                $"Maximum recommended is {MaxWeeklyWeightChangeKg} kg per week."));
        }

        var bmr = CalculateBmr(metabolicProfile);
        var tdee = CalculateTdee(bmr, metabolicProfile.ExerciseDaysPerWeek);

        // Calculate daily caloric adjustment needed
        var calorieAdjustment = (int)((weeklyWeightChange * CaloriesPerKg) / 7);
        var targetCalories = tdee + calorieAdjustment;

        // Check minimum safe calories
        var minSafeCalories = metabolicProfile.IsMale ? 1500 : 1200;
        if (targetCalories < minSafeCalories)
        {
            return Result.Fail<int>(Error.Validation(
                $"Target calories ({targetCalories}) are below safe minimum of {minSafeCalories}. " +
                "Consider a longer time period to reach your goal."));
        }

        return Result.Success(targetCalories);
    }

    private Result ValidateMetabolicProfile(MetabolicProfileDto request)
    {
        if (request.WeightKg is < MinWeightKg or > MaxWeightKg)
            return Result.Fail(Error.Validation($"Weight must be between {MinWeightKg} and {MaxWeightKg} kg"));

        if (request.HeightCm is < MinHeightCm or > MaxHeightCm)
            return Result.Fail(Error.Validation($"Height must be between {MinHeightCm} and {MaxHeightCm} cm"));

        if (request.Age is < MinAge or > MaxAge)
            return Result.Fail(Error.Validation($"Age must be between {MinAge} and {MaxAge} years"));

        if (request.ExerciseDaysPerWeek is < MinExerciseDays or > MaxExerciseDays)
            return Result.Fail(Error.Validation($"Exercise days must be between {MinExerciseDays} and {MaxExerciseDays}"));

        return Result.Success();
    }

    private Result ValidateGoalRequest(WeightGoalPlanDto request)
    {
        var basicValidation = this.ValidateMetabolicProfile(request.MetabolicProfile);
        if (basicValidation.IsFailure)
            return basicValidation;

        if (request.TargetWeeks < MinTargetWeeks)
            return Result.Fail(Error.Validation($"Target period must be at least {MinTargetWeeks} weeks"));

        if (request.TargetWeightKg is < MinWeightKg or > MaxWeightKg)
            return Result.Fail(Error.Validation($"Target weight must be between {MinWeightKg} and {MaxWeightKg} kg"));

        return Result.Success();
    }

    private static int CalculateBmr(MetabolicProfileDto request)
    {
        // Mifflin-St Jeor Formula
        var bmr = (10 * request.WeightKg) + (6.25 * request.HeightCm) - (5 * request.Age);
        bmr = request.IsMale ? bmr + 5 : bmr - 161;

        return (int)Math.Round(bmr);
    }

    private static int CalculateTdee(int bmr, int exerciseDaysPerWeek)
    {
        // Activity multiplier based on exercise frequency
        var activityMultiplier = exerciseDaysPerWeek switch
        {
            0 => 1.2, // Sedentary
            1 or 2 => 1.375, // Lightly active
            3 or 4 => 1.55, // Moderately active
            5 or 6 => 1.725, // Very active
            7 => 1.9, // Extra active
            _ => 1.2
        };

        return (int)Math.Round(bmr * activityMultiplier);
    }
}