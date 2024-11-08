namespace DiamondShop.Repository.ViewModels.Response.Account;

public class GetAccountResponse
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;
}