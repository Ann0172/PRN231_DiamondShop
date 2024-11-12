using DiamondShop.Repository;
using DiamondShop.Repository.Interfaces;
using DiamondShop.Repository.Models;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Response.Certificate;
using DiamondShop.Service.Interfaces;
using Mapster;

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
    
}   