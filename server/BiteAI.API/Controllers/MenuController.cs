using System.Security.Claims;
using BiteAI.API.Controllers.Base;
using BiteAI.Infrastructure.Constants;
using BiteAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiteAI.API.Controllers;

[Authorize]
[Route("api/menu")]
public class MenuController : BaseApiController
{
    private readonly IGroceryListService _groceryListService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MenuController(IGroceryListService groceryListService, IHttpContextAccessor httpContextAccessor)
    {
        _groceryListService = groceryListService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("{menuId}/grocery-list")]
    public async Task<IActionResult> GenerateGroceryList([FromRoute] string menuId, CancellationToken cancellationToken = default)
    {
        var loggedInUserId = this._httpContextAccessor.HttpContext?.User.FindFirstValue(ExtendedClaimTypes.UniqueIdentifier);
        if (string.IsNullOrEmpty(loggedInUserId))
            return this.NoLoggedInUserResult();

        var result = await _groceryListService.GenerateGroceryListAsync(menuId, loggedInUserId, cancellationToken);
        return this.ToActionResult(result);
    }
}
