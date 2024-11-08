using DiamondShop.Repository.ViewModels.Response.Account;

namespace DiamondShop.Repository.ViewModels.Response.Order;

public class GetAllOrderResponse
{
    public Guid Id { get; set; }

    public string? PayMethod { get; set; }

    public string Status { get; set; } = null!;

    public DateOnly CreatedDate { get; set; }

    public long TotalPrice { get; set; }

    public GetAccountResponse? Customer { get; set; }

    public GetAccountResponse? SalesStaff { get; set; }

    public GetAccountResponse? DeliveryStaff { get; set; }
}