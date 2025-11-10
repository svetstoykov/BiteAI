using BiteAI.API.Controllers.Base;
using BiteAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiteAI.API.Controllers;

[AllowAnonymous]
[Route("api/enums")]
public class EnumsController(IEnumLookupService enums) : BaseApiController
{
    [HttpGet("activity-levels")]
    public async Task<IActionResult> GetActivityLevels(CancellationToken ct = default)
        => this.ToActionResult(await enums.GetActivityLevelsAsync(ct));

    [HttpGet("diet-types")]
    public async Task<IActionResult> GetDietTypes(CancellationToken ct = default)
        => this.ToActionResult(await enums.GetDietTypesAsync(ct));

    [HttpGet("genders")]
    public async Task<IActionResult> GetGenders(CancellationToken ct = default)
        => this.ToActionResult(await enums.GetGendersAsync(ct));

    [HttpGet("meal-types")]
    public async Task<IActionResult> GetMealTypes(CancellationToken ct = default)
        => this.ToActionResult(await enums.GetMealTypesAsync(ct));

    [HttpGet("allergies")]
    public async Task<IActionResult> GetAllergies(CancellationToken ct = default)
        => this.ToActionResult(await enums.GetAllergiesAsync(ct));

    [HttpGet("food-dislikes")]
    public async Task<IActionResult> GetFoodDislikes(CancellationToken ct = default)
        => this.ToActionResult(await enums.GetFoodDislikesAsync(ct));
}