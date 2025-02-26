using System.ComponentModel.DataAnnotations;

namespace BiteAI.API.Models.DTOs.Authentication;

public class LoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}
