using System.ComponentModel.DataAnnotations;

namespace DiamondShop.Repository.ViewModels.Request.Auth;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
}