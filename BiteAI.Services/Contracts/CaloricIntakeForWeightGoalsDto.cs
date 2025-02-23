namespace BiteAI.Services.Contracts;

public class CaloricIntakeForWeightGoalsDto
{
    public int MaintainCalories { get; set; }
    public int LoseOneKgCalories { get; set; }
    public int LoseHalfKgCalories { get; set; }
    public int GainHalfKgCalories { get; set; }
    public int GainOneKgCalories { get; set; }
}