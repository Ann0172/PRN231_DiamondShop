using DiamondShop.Repository;
using DiamondShop.Repository.Interfaces;
using DiamondShop.Repository.Models;
using DiamondShop.Repository.ViewModels.Response.Category;
using DiamondShop.Repository.ViewModels.Response.Diamond;
using DiamondShop.Repository.ViewModels.Response.OrderDetail;
using DiamondShop.Repository.ViewModels.Response.Picture;
using DiamondShop.Repository.ViewModels.Response.Product;
using DiamondShop.Service.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DiamondShop.Service.Services;

public class OrderDetailService : IOrderDetailService
{
    private readonly IUnitOfWork<Prn231DiamondShopContext> _unitOfWork;

    public OrderDetailService(IUnitOfWork<Prn231DiamondShopContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<GetOrderDetailResponse>> GetOrderDetailByOrderId(Guid orderId)
    {
        var orderDetails = await _unitOfWork.GetRepository<OrderDetail>().GetListAsync(
            predicate: x => x.OrderId == orderId,
            include: x =>
                x.Include(o => o.Product).ThenInclude(p => p.Pictures));
        return orderDetails.Adapt<List<GetOrderDetailResponse>>();
    }
}