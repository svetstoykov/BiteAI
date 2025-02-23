namespace BiteAI.Services.Entities;

public class Meal
{
    public string Name { get; set; }
    public List<string> Ingredients { get; set; }
    public string PreparationSteps { get; set; }
    public int Calories { get; set; }
    public Dictionary<string, double> Macros { get; set; } // Proteins, Car
}