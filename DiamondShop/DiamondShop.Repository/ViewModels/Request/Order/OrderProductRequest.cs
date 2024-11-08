namespace DiamondShop.Repository.ViewModels.Request.Order;

public class OrderProductRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}