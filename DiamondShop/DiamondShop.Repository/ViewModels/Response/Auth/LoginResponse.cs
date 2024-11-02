namespace DiamondShop.Repository.ViewModels.Response.Auth;

public class LoginResponse
{
    public required string AccessToken { get; set; }

    public required DateTime ExpireIn { get; set; }
}