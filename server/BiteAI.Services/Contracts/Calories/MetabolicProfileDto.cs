namespace BiteAI.Services.Contracts.Calories;

public class MetabolicProfileDto
{
    public double WeightKg { get; set; }
    public double HeightCm { get; set; }
    public int Age { get; set; }
    public int ExerciseDaysPerWeek { get; set; }
    public bool IsMale { get; set; }
}