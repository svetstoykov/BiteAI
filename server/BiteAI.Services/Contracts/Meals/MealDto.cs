using System.ComponentModel.DataAnnotations;
using BiteAI.Services.Contracts.Enums;

namespace BiteAI.Services.Contracts.Meals;

public class MealDto
{
    [MaxLength(30)]
    public string Name { get; set; }
    public string Recipe { get; set; }
    public int Calories { get; set; }
    public MealTypes MealType { get; set; }
    public int ProteinInGrams { get; set; }
    public int CarbsInGrams { get; set; }
    public int FatInGrams { get; set; }
}