using BiteAI.Services.Contracts;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Interfaces;

public interface ICalorieService
{
    Result<CaloricIntakeForWeightGoalsDto> CalculateCalorieGoals(MetabolicProfileDto request);
    
    Result<int> CalculateTargetDailyCalories(WeightGoalPlanDto request);
}