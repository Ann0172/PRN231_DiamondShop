using DiamondShop.Repository.ViewModels.Response.Picture;

namespace DiamondShop.Repository.ViewModels.Response.Product;

public class GetProductResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public ICollection<GetPictureResponse> Pictures { get; set; } = [];
}