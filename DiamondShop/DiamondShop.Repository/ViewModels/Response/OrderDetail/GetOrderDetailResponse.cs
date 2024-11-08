using DiamondShop.Repository.ViewModels.Response.Product;

namespace DiamondShop.Repository.ViewModels.Response.OrderDetail;

public class GetOrderDetailResponse
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }

    public decimal Price { get; set; }
    public GetProductResponse? Product { get; set; }
}