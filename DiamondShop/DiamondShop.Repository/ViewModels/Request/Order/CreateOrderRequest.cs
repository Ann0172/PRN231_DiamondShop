using System.ComponentModel.DataAnnotations;

namespace DiamondShop.Repository.ViewModels.Request.Order;

public class CreateOrderRequest
{
    public List<OrderProductRequest> ListOrder { get; set; } = [];
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Address { get; set; }
}