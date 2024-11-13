using DiamondShop.Repository.ViewModels.Response.OrderDetail;
using DiamondShop.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShop.API.Controllers
{
    [Route("api/order-details")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }
        [HttpGet("get-by-order-id/{orderId:guid}")]
        [Authorize(Roles = "Customer, DeliveryStaff, SalesStaff")]
        public async Task<ActionResult<List<GetOrderDetailResponse>>> GetByOrderId(Guid orderId)
        {
            return await _orderDetailService.GetOrderDetailByOrderId(orderId);
        }
    }
}
