using DiamondShop.Repository.ViewModels.Response.OrderDetail;

namespace DiamondShop.Service.Interfaces;

public interface IOrderDetailService
{
    Task<List<GetOrderDetailResponse>> GetOrderDetailByOrderId(Guid orderId);
}