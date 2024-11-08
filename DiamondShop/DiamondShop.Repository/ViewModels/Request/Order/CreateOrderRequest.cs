namespace DiamondShop.Repository.ViewModels.Request.Order;

public class CreateOrderRequest
{
    public List<OrderProductRequest> ListOrder { get; set; } = [];
    public required string PayMethod { get; set; }
}