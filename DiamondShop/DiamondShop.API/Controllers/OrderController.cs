using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Order;
using DiamondShop.Repository.ViewModels.Response.Order;
using DiamondShop.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShop.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("cash")]
        public async Task<ActionResult<GetOrderByCashResponse>> CreateOrderWithCash([FromBody]CreateOrderRequest createOrderRequest)
        {
            return Created(nameof(CreateOrderWithCash), await _orderService.CreateOrderWithCash(HttpContext.User, createOrderRequest));
        }
        [HttpPost("payos")]
        public async Task<ActionResult<GetOrderByPayOsResponse>> CreateOrderWithPayOs([FromBody]CreateOrderRequest createOrderRequest)
        {
            return Created(nameof(CreateOrderWithPayOs), await _orderService.CreateOrderWithPayOs(HttpContext.User, createOrderRequest));
        }

        [HttpPatch("update-status-for-order/{orderId:guid}/{status}")]
        public async Task<ActionResult> UpdateOrderStatus(Guid orderId, OrderStatus status, [FromQuery] Guid deliveryStaffId)
        {
            await _orderService.UpdateOrderStatus(HttpContext.User, orderId, status, deliveryStaffId);
            return NoContent();
        }
        [HttpGet("success")]
        public async Task<IActionResult> Success([FromQuery] Guid orderId,[FromQuery] string status)
        {
            if (status == "CANCELLED")
            {
                await _orderService.UpdateOrderStatusForPayOs(orderId, OrderStatus.Cancelled);
                return Redirect("https://app.gradient.network/dashboard");
            }
            await _orderService.UpdateOrderStatusForPayOs(orderId, OrderStatus.Confirmed); 
            return Redirect("https://app.gradient.network/dashboard");
        }

        [HttpGet("get-orders")]
        public async Task<ActionResult<Paginate<GetAllOrderResponse>>> GetPagedOrder([FromQuery] OrderStatus orderStatus,
            [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            return await _orderService.GetAllOrderByAccount(HttpContext.User, orderStatus, page, size);
        }
    }
}
