using System.Security.Claims;
using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Order;
using DiamondShop.Repository.ViewModels.Response.Order;

namespace DiamondShop.Service.Interfaces;

public interface IOrderService
{
    Task<GetOrderByCashResponse> CreateOrderWithCash(ClaimsPrincipal claims,CreateOrderRequest createOrderRequest);
    Task<GetOrderByPayOsResponse> CreateOrderWithPayOs(ClaimsPrincipal claims, CreateOrderRequest createOrderRequest);
    Task UpdateOrderStatus(ClaimsPrincipal claimsPrincipal, Guid orderId, OrderStatus status, Guid deliveryStaffId);
    Task UpdateOrderStatusForPayOs(Guid orderId, OrderStatus status);

    Task<Paginate<GetAllOrderResponse>> GetAllOrderByAccount(ClaimsPrincipal claims, OrderStatus status, int page, int size);
}