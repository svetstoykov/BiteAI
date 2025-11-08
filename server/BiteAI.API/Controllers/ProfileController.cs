using BiteAI.API.Controllers.Base;
using BiteAI.Services.Contracts.Profiles;
using BiteAI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiteAI.API.Controllers;

[Authorize]
[Route("api/profile")]
public class ProfileController(IUserProfileService profileService) : BaseApiController
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken = default)
        => this.ToActionResult(await profileService.GetMyProfileAsync(cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserProfileDto dto, CancellationToken cancellationToken = default)
    {
        var result = await profileService.CreateMyProfileAsync(dto, cancellationToken);
        if (result.IsFailure) return this.ToActionResult(result);
        return this.Created($"/api/profile/me", new { data = result.Data });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserProfileDto dto, CancellationToken cancellationToken = default)
        => this.ToActionResult(await profileService.UpdateMyProfileAsync(dto, cancellationToken));

    [HttpDelete]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken = default)
        => this.ToActionResult(await profileService.DeleteMyProfileAsync(cancellationToken));

    [HttpGet("exists")]
    public async Task<IActionResult> Exists(CancellationToken cancellationToken = default)
        => this.ToActionResult(await profileService.HasProfileAsync(cancellationToken));
}
