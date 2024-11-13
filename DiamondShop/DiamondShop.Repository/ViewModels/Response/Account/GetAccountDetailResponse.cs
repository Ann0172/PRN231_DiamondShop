namespace DiamondShop.Repository.ViewModels.Response.Account;

public class GetAccountDetailResponse
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? AvatarUrl { get; set; }

    public string? Address { get; set; }

    public string Role { get; set; } = null!;

    public string Status { get; set; } = null!;
}