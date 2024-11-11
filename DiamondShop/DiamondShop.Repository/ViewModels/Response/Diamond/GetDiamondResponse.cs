namespace DiamondShop.Repository.ViewModels.Response.Diamond;

public class GetDiamondResponse
{
    public Guid Id { get; set; }
    public string? Shape { get; set; }
    public string? CaratWeight { get; set; }
    public string? Clarity { get; set; }
}