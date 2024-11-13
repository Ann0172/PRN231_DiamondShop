using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using DiamondShop.Repository;
using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Interfaces;
using DiamondShop.Repository.Models;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Response.Certificate;
using DiamondShop.Repository.ViewModels.Response.Product;
using DiamondShop.Service.Extensions;
using DiamondShop.Service.Interfaces;
using DiamondShop.Shared.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DiamondShop.Service.Services;

public class CertificateService : ICertificateService
{
    private readonly IUnitOfWork<Prn231DiamondShopContext> _unitOfWork;

    public CertificateService(IUnitOfWork<Prn231DiamondShopContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Paginate<GetCertificatePagedResponse>> GetPagedCertificate(int page, int size)
    {
        var certificates = await _unitOfWork.GetRepository<Certificate>().GetPagingListAsync(page: page, size: size);
        return certificates.Adapt<Paginate<GetCertificatePagedResponse>>();
    }

    public async Task<GetCertificateProductPagedResponse> GetCertificateForAccount(ClaimsPrincipal claim)

    {
        var accountId = claim.GetAccountId();
        var account = await _unitOfWork.GetRepository<Account>()
            .SingleOrDefaultAsync(predicate: o => o.Id == accountId);
        if (account is null)
        {
            throw new UnauthorizedException("Invalid account");
        }

        var orders = await _unitOfWork.GetRepository<Order>().GetListAsync(
            predicate: x => x.Status == OrderStatus.Deliveried.ToString() && x.CustomerId == accountId,
            include: x => x
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Diamond)
                .ThenInclude(d => d.Certificate)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Pictures)
        );
        
        var orderDetails = new List<OrderDetail>();
        foreach (var order in orders)
        {
            orderDetails.AddRange(order.OrderDetails);
        }
        var certificateProductResponses = new List<CertificateProductResponse>();
        var addedProductIds = new HashSet<Guid>();
        foreach (var orderDetail in orderDetails)
        {
            if (addedProductIds.Contains(orderDetail.Product.Id)) continue;
            certificateProductResponses.Add(new CertificateProductResponse
            {
                Certificate = orderDetail.Product.Diamond.Certificate.Adapt<GetCertificatePagedResponse>(),
                Product = orderDetail.Product.Adapt<GetProductResponse>()
            });
            addedProductIds.Add(orderDetail.Product.Id);
        }

        return new GetCertificateProductPagedResponse()
        {
            Items = certificateProductResponses,
        };
    }
}   