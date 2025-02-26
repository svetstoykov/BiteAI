using BiteAI.Services.Contracts.Authentication;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Interfaces;

public interface IIdentityService
{
    /// <summary>
    /// Registers a new user in the system
    /// </summary>
    /// <param name="model">Registration information</param>
    /// <returns>Result containing user ID if successful</returns>
    Task<Result<RegisterResponseDto>> RegisterUserAsync(RegisterDto model);
    
    /// <summary>
    /// Authenticates a user and generates a JWT token
    /// </summary>
    /// <param name="model">Login credentials</param>
    /// <returns>Result containing login information including JWT token if successful</returns>
    Task<Result<LoginResponseDto>> LoginUserAsync(LoginDto model);
}