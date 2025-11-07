using System.Security.Claims;
using BiteAI.API.Controllers.Base;
using BiteAI.API.Models.Groceries;
using BiteAI.Infrastructure.Constants;
using BiteAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiteAI.API.Controllers;

[Authorize]
[Route("api/grocery-list")]
public class GroceryListController(IGroceryListService groceryListService, IHttpContextAccessor httpContextAccessor)
    : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetLatestGroceryListForUser(CancellationToken cancellationToken = default)
    {
        var loggedInUserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ExtendedClaimTypes.UniqueIdentifier);
        if (string.IsNullOrEmpty(loggedInUserId))
            return this.NoLoggedInUserResult();

        var result = await groceryListService.GetLatestGroceryListForUserAsync(loggedInUserId, cancellationToken);
        return this.ToActionResult(result);
    }

    [HttpGet("{mealPlanId:guid}")]
    public async Task<IActionResult> GetGroceryList([FromRoute] Guid mealPlanId, CancellationToken cancellationToken = default)
    {
        var loggedInUserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ExtendedClaimTypes.UniqueIdentifier);
        if (string.IsNullOrEmpty(loggedInUserId))
            return this.NoLoggedInUserResult();

        var result = await groceryListService.GetGroceryListAsync(mealPlanId, loggedInUserId, cancellationToken);
        return this.ToActionResult(result);
    }

    [HttpPost("{mealPlanId:guid}")]
    public async Task<IActionResult> GenerateGroceryList([FromRoute] Guid mealPlanId, CancellationToken cancellationToken = default)
    {
        var loggedInUserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ExtendedClaimTypes.UniqueIdentifier);
        if (string.IsNullOrEmpty(loggedInUserId))
            return this.NoLoggedInUserResult();

        var result = await groceryListService.GenerateGroceryListAsync(mealPlanId, loggedInUserId, cancellationToken);
        return this.ToActionResult(result);
    }

    [HttpPost("check-items")]
    public async Task<IActionResult> ToggleGroceryItemsChecked([FromBody] ToggleGroceryItemsRequest request, CancellationToken cancellationToken = default)
    {
        var loggedInUserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ExtendedClaimTypes.UniqueIdentifier);
        if (string.IsNullOrEmpty(loggedInUserId))
            return this.NoLoggedInUserResult();

        var result = await groceryListService.ToggleGroceryItemsCheckedAsync(request.ItemIds, loggedInUserId, cancellationToken);
        return this.ToActionResult(result);
    }
}
