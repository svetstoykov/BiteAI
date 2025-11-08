using BiteAI.Services.Contracts.Profiles;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Interfaces;

public interface IUserProfileService
{
    Task<Result<UserProfileDto?>> GetMyProfileAsync(CancellationToken cancellationToken = default);
    Task<Result<UserProfileDto?>> GetProfileByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<UserProfileDto?>> CreateMyProfileAsync(CreateUserProfileDto dto, CancellationToken cancellationToken = default);
    Task<Result<UserProfileDto?>> UpdateMyProfileAsync(UpdateUserProfileDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteMyProfileAsync(CancellationToken cancellationToken = default);
    Task<Result<bool>> HasProfileAsync(CancellationToken cancellationToken = default);
}
