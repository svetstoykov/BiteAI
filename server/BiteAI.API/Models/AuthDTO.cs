using System.ComponentModel.DataAnnotations;
using BiteAI.Infrastructure.Models;

namespace BiteAI.API.Models;

public class LoginDTO
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}

public class RegisterDTO
{
    [Required(ErrorMessage = "First name is required")]
    public required string FirstName { get; set; }
        
    [Required(ErrorMessage = "Last name is required")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public required string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }
        
    // Optional user profile fields
    public Gender Gender { get; set; }
    public int? Age { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
    public ActivityLevel ActivityLevel { get; set; } = ActivityLevel.NotSpecified;
}

public class LoginResponseDTO
{
    public required string Token { get; set; }
    public string? RefreshToken { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public DateTime TokenExpiration { get; set; }
    public Guid UserId { get; set; }
}