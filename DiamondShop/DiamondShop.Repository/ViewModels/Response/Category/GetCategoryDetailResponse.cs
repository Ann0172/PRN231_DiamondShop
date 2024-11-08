namespace DiamondShop.Repository.ViewModels.Response.Category;

public class GetCategoryDetailResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? LastUpdate { get; set; }

    public string? Status { get; set; }
}