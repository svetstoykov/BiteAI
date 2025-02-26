using BiteAI.Services.Contracts.UserProfile;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Interfaces;

public interface IUsersService
{
    Task<Result<UserProfileDto>> GetUserProfileAsync(string userId);
    Task<Result<bool>> UpdateUserProfileAsync(string userId, UserProfileUpdateDto model);
}