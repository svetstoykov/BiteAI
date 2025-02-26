using BiteAI.Infrastructure.Data;
using BiteAI.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BiteAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MealPlanController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public MealPlanController(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserMealPlans()
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                     throw new UnauthorizedAccessException("User ID not found in claims");

        var mealPlans = await this._dbContext.Set<MealPlan>()
            .Where(mp => mp.UserId == userId)
            .OrderByDescending(mp => mp.CreatedDate)
            .Select(mp => new
            {
                mp.Id,
                mp.Name,
                mp.CreatedDate,
                mp.DailyCalories,
                mp.DietType,
                mp.DurationDays
            })
            .ToListAsync();

        return this.Ok(mealPlans);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMealPlanDetails(int id)
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                     throw new UnauthorizedAccessException("User ID not found in claims");

        var mealPlan = await this._dbContext.Set<MealPlan>()
            .Where(mp => mp.Id == id && mp.UserId == userId)
            .Include(mp => mp.MealDays)
            .ThenInclude(md => md.Meals)
            .FirstOrDefaultAsync();

        if (mealPlan == null)
        {
            return this.NotFound();
        }

        return this.Ok(new
        {
            mealPlan.Id,
            mealPlan.Name,
            mealPlan.CreatedDate,
            mealPlan.DailyCalories,
            DietType = mealPlan.DietType.ToString(),
            mealPlan.DurationDays,
            Days = mealPlan.MealDays.OrderBy(md => md.DayNumber).Select(md => new
            {
                md.Id,
                md.DayNumber,
                md.Date,
                md.TotalCalories,
                Meals = md.Meals.Select(m => new
                {
                    m.Id,
                    m.Name,
                    m.Description,
                    m.Recipe,
                    m.Calories,
                    MealType = m.MealType.ToString(),
                    m.Protein,
                    m.Carbs,
                    m.Fat
                })
            })
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateMealPlan([FromBody] CreateMealPlanDto model)
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                     throw new UnauthorizedAccessException("User ID not found in claims");

        var mealPlan = new MealPlan
        {
            Name = model.Name,
            CreatedDate = DateTime.UtcNow,
            DailyCalories = model.DailyCalories,
            DietType = model.DietType,
            DurationDays = model.DurationDays,
            UserId = userId
        };

        this._dbContext.Add(mealPlan);
        await this._dbContext.SaveChangesAsync();

        return this.CreatedAtAction(nameof(this.GetMealPlanDetails), new { id = mealPlan.Id }, new { mealPlan.Id });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMealPlan(int id)
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                     throw new UnauthorizedAccessException("User ID not found in claims");

        var mealPlan = await this._dbContext.Set<MealPlan>()
            .Where(mp => mp.Id == id && mp.UserId == userId)
            .FirstOrDefaultAsync();

        if (mealPlan == null)
        {
            return this.NotFound();
        }

        this._dbContext.Remove(mealPlan);
        await this._dbContext.SaveChangesAsync();

        return this.NoContent();
    }
}

public class CreateMealPlanDto
{
    public required string Name { get; set; }
    public int DailyCalories { get; set; }
    public DietType DietType { get; set; } = DietType.Standard;
    public int DurationDays { get; set; } = 7;
}