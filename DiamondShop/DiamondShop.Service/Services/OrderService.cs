﻿using System.Linq.Expressions;
using System.Security.Claims;
using DiamondShop.Repository;
using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Interfaces;
using DiamondShop.Repository.Models;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Order;
using DiamondShop.Repository.ViewModels.Response.Order;
using DiamondShop.Service.Extensions;
using DiamondShop.Service.Interfaces;
using DiamondShop.Shared.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using Net.payOS.Types;

namespace DiamondShop.Service.Services;

public class OrderService : IOrderService
{
    private readonly PayOS _payOs;
    private readonly IUnitOfWork<Prn231DiamondShopContext> _unitOfWork;

    public OrderService(IUnitOfWork<Prn231DiamondShopContext> unitOfWork, PayOS payOs)
    {
        _unitOfWork = unitOfWork;
        _payOs = payOs;
    }
    public async Task<GetOrderByCashResponse> CreateOrderWithCash(ClaimsPrincipal claims, CreateOrderRequest createOrderRequest)
    {
        var accountId = claims.GetAccountId();
        var totalPrice = 0.0;
        foreach (var x in createOrderRequest.ListOrder)
        {
            var product = await _unitOfWork.GetRepository<Product>()
                .SingleOrDefaultAsync(predicate: p => p.Id == x.ProductId);
            totalPrice += (double)product.Price;
        }

        var order = new Order
        {
            CustomerId = accountId,
            PayMethod = Payment.CASH.ToString(),
            Status = OrderStatus.Pending.ToString(),
            CreatedDate = DateOnly.FromDateTime(DateTime.Now),
            TotalPrice = (long)totalPrice
        };
        await _unitOfWork.GetRepository<Order>().InsertAsync(order);
        await _unitOfWork.CommitAsync();
        var orderDetails = new List<OrderDetail>();
        foreach (var x in createOrderRequest.ListOrder)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = order.Id,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Price = (await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(predicate:p => p.Id == x.ProductId)).Price
            };
            orderDetails.Add(orderDetail);
        }

        await _unitOfWork.GetRepository<OrderDetail>().InsertRangeAsync(orderDetails);
        await _unitOfWork.CommitAsync();
        return new GetOrderByCashResponse { Id = order.Id };
    }

    public async Task<GetOrderByPayOsResponse> CreateOrderWithPayOs(ClaimsPrincipal claims, CreateOrderRequest createOrderRequest)
    {
        var accountId = claims.GetAccountId();
        var totalPrice = 0.0;
        foreach (var x in createOrderRequest.ListOrder)
        {
            var product = await _unitOfWork.GetRepository<Product>()
                .SingleOrDefaultAsync(predicate: p => p.Id == x.ProductId);
            totalPrice += (double)product.Price;
        }

        var order = new Order
        {
            CustomerId = accountId,
            PayMethod = Payment.PAYOS.ToString(),
            Status = OrderStatus.Confirmed.ToString(),
            CreatedDate = DateOnly.FromDateTime(DateTime.Now),
            TotalPrice = (long)totalPrice
        };
        await _unitOfWork.GetRepository<Order>().InsertAsync(order);
        await _unitOfWork.CommitAsync();
        var orderDetails = new List<OrderDetail>();
        foreach (var x in createOrderRequest.ListOrder)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = order.Id,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Price = (await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(predicate:p => p.Id == x.ProductId)).Price
            };
            orderDetails.Add(orderDetail);
        }

        await _unitOfWork.GetRepository<OrderDetail>().InsertRangeAsync(orderDetails);
        await _unitOfWork.CommitAsync();
        var orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
        var items = new List<ItemData>();
        var orderDetailDb = await _unitOfWork.GetRepository<OrderDetail>()
            .GetListAsync(predicate: o => o.OrderId == order.Id, include: x => x.Include(o => o.Product));
        foreach (var x in orderDetailDb)
        {
            var item = new ItemData(x.Product.Name, x.Quantity, (int)x.Price);
            items.Add(item);
        }
        const string baseUrl = "https://localhost:7198/api/orders/" + "/success";
        var url = $"{baseUrl}?&orderId={order.Id}";
        var paymentData = new PaymentData(orderCode, (int)(order.TotalPrice), "Pay Order", items,
            url, url);
        var createPayment = await _payOs.createPaymentLink(paymentData);
        return new GetOrderByPayOsResponse
        {
            Message = "Order Successfully",
            Url = createPayment.checkoutUrl
        };
    }

    // public async Task UpdateOrderStatusWithStaff(ClaimsPrincipal claimsPrincipal, Guid orderId, OrderStatus status)
    // {
    //     var accountId = claimsPrincipal.GetAccountId();
    //     var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate:a => a.Id == accountId);
    //     if (account is null)
    //     {
    //         throw new UnauthorizedException("Invalid account!!!!!");
    //     }
    //     var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(predicate: x => x.Id == orderId);
    //     order.Status = status switch
    //     {
    //         OrderStatus.Cancelled => OrderStatus.Cancelled.ToString(),
    //         OrderStatus.Confirmed => OrderStatus.Confirmed.ToString(),
    //         OrderStatus.Pending => OrderStatus.Pending.ToString(),
    //         OrderStatus.WaitToDelivery => OrderStatus.WaitToDelivery.ToString(),
    //         _ => order.Status
    //     };
    //     order.SalesStaffId = accountId;
    //     await _unitOfWork.CommitAsync();
    // }
    public async Task UpdateOrderStatus(ClaimsPrincipal claimsPrincipal, Guid orderId, OrderStatus status)
    {
        var accountId = claimsPrincipal.GetAccountId();
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: a => a.Id == accountId);
        if (account is null)
        {
            throw new UnauthorizedException("Invalid account!!!!!");
        }

        var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(predicate: x => x.Id == orderId);
        if (order is null)
        {
            throw new NotFoundException("Order not found");
        }
        if (account.Role == Role.SalesStaff.ToString())
        {
            if (status != OrderStatus.Cancelled && status != OrderStatus.Confirmed &&
                status != OrderStatus.Pending && status != OrderStatus.WaitToDelivery)
            {
                throw new BadRequestException("Sale Staff cannot change to this order status");
            }
            order.Status = status.ToString();
            order.SalesStaffId = accountId;
        }
        else if (account.Role == Role.DeliveryStaff.ToString())
        {
            if (status != OrderStatus.Deliveried && status != OrderStatus.Delivering)
            {
                throw new BadRequestException("Delivery staff cannot change to this order status");
            }

            order.Status = status.ToString();
            order.DeliveryStaffId = accountId;
        }
        _unitOfWork.GetRepository<Order>().UpdateAsync(order);
        await _unitOfWork.CommitAsync();
    }

    // public async Task UpdateOrderStatusWithDeliveryStaff(ClaimsPrincipal claimsPrincipal, Guid orderId, OrderStatusForDelivery status)
    // {
    //     var accountId = claimsPrincipal.GetAccountId();
    //     var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate:a => a.Id == accountId);
    //     if (account is null)
    //     {
    //         throw new UnauthorizedException("Invalid account!!!!!");
    //     }
    //     var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(predicate: x => x.Id == orderId);
    //     order.Status = status switch
    //     {
    //         OrderStatusForDelivery.Deliveried => OrderStatusForDelivery.Deliveried.ToString(),
    //         OrderStatusForDelivery.Delivering => OrderStatusForDelivery.Delivering.ToString(),
    //         _ => order.Status
    //     };
    //     order.DeliveryStaffId = accountId;
    //     await _unitOfWork.CommitAsync();
    // }

    public async Task UpdateOrderStatusForPayOs(Guid orderId, OrderStatus status)
    {
        var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(predicate: x => x.Id == orderId);
        order.Status = status switch
        {
            OrderStatus.Cancelled => OrderStatus.Cancelled.ToString(),
            OrderStatus.Confirmed => OrderStatus.Confirmed.ToString(),
            _ => order.Status
        };
        _unitOfWork.GetRepository<Order>().UpdateAsync(order);
        await _unitOfWork.CommitAsync();
    }

    public async Task<Paginate<GetAllOrderResponse>> GetAllOrderByAccount(ClaimsPrincipal claims, OrderStatus status, int page, int size)
    {
        var accountId = claims.GetAccountId();
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: a => a.Id == accountId);
        if (account is null)
        {
            throw new UnauthorizedException("Invalid account!!!");
            
        }
        Expression<Func<Order, bool>> predicate = x => x.Status == status.ToString();

        if (account.Role == Role.Customer.ToString())
        {
            predicate = predicate.And(x => x.CustomerId == accountId);
        }
        else if (account.Role == Role.DeliveryStaff.ToString())
        {
            predicate = predicate.And(x => x.DeliveryStaffId == accountId);
        }
        var orders = await _unitOfWork.GetRepository<Order>().GetPagingListAsync(
            predicate: predicate,
            include: x => x.Include(o => o.Customer)
                .Include(o => o.DeliveryStaff)
                .Include(o => o.SalesStaff),
            size: size, page: page, orderBy: x => x.OrderByDescending(o => o.CreatedDate));

        return orders.Adapt<Paginate<GetAllOrderResponse>>();
    }
}