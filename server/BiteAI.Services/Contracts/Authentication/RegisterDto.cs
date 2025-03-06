using System.ComponentModel.DataAnnotations;
using BiteAI.Services.Contracts.Enums;

namespace BiteAI.Services.Contracts.Authentication;


public class RegisterDto
{
    [Required(ErrorMessage = "First name is required")]
    public required string FirstName { get; set; }
        
    [Required(ErrorMessage = "Last name is required")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }
        
    public Genders? Gender { get; set; }
    
    public int? Age { get; set; }
    public double? WeightInKg { get; set; }
    public double? HeightInCm { get; set; }
    public ActivityLevels? ActivityLevel { get; set; }
}