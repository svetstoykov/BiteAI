namespace BiteAI.Infrastructure.Models;

public class Meal
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Recipe { get; set; }
    public int Calories { get; set; }
    public MealType MealType { get; set; }
    public int Protein { get; set; } // in grams
    public int Carbs { get; set; } // in grams
    public int Fat { get; set; } // in grams
        
    // Foreign key for meal day
    public int MealDayId { get; set; }
    public virtual MealDay? MealDay { get; set; }
}
    
public enum MealType
{
    Breakfast = 0,
    Lunch = 1,
    Dinner = 2,
    Snack = 3
}