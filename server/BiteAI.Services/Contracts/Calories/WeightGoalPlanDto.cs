namespace BiteAI.Services.Contracts.Calories;

public class WeightGoalPlanDto
{
    public MetabolicProfileDto MetabolicProfile { get; set; }
    
    public double TargetWeightKg { get; set; }
    
    public int TargetWeeks { get; set; }
}