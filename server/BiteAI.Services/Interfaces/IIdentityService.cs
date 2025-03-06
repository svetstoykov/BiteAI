using BiteAI.Services.Contracts.Authentication;
using BiteAI.Services.Data.Entities;
using BiteAI.Services.Validation.Result;

namespace BiteAI.Services.Interfaces;

public interface IIdentityService
{
    /// <summary>
    /// Registers a new user in the system with the provided credentials and information
    /// </summary>
    /// <param name="model">Registration information containing user details</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete (optional)</param>
    /// <returns>A Result object containing RegisterResponseDto with the newly created user ID if registration is successful</returns>
    /// <remarks>This method validates the registration data before creating the account</remarks>
    Task<Result<RegisterResponseDto>> RegisterUserAsync(RegisterDto model, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Authenticates a user with the provided credentials and generates a JWT token for authorization
    /// </summary>
    /// <param name="model">Login credentials containing username/email and password</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete (optional)</param>
    /// <returns>A Result object containing LoginResponseDto with JWT token and expiration information if authentication is successful</returns>
    /// <remarks>The returned token should be included in the Authorization header for subsequent API calls</remarks>
    Task<Result<LoginResponseDto>> LoginUserAsync(LoginDto model, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves the currently authenticated user's information
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete (optional)</param>
    /// <returns>A Result object containing the ApplicationUser entity if the user is authenticated</returns>
    /// <remarks>This method uses the current ClaimsPrincipal identity to determine the logged-in user</remarks>
    Task<Result<ApplicationUser>> GetLoggedInUserAsync(CancellationToken cancellationToken = default);
}