using DiamondShop.Repository;
using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Interfaces;
using DiamondShop.Repository.Models;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Diamond;
using DiamondShop.Repository.ViewModels.Response.Diamond;
using DiamondShop.Service.Interfaces;
using DiamondShop.Shared.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DiamondShop.Service.Services;

public class DiamondService : IDiamondService
{
    private readonly IUnitOfWork<Prn231DiamondShopContext> _unitOfWork;
    private readonly IPictureService _pictureService;

    public DiamondService(IUnitOfWork<Prn231DiamondShopContext> unitOfWork, IPictureService pictureService)
    {
        _unitOfWork = unitOfWork;
        _pictureService = pictureService;
    }
    public async Task<GetDiamondId> CreateDiamond(CreateDiamondRequest createDiamondRequest)
    {
        var certificate = await _unitOfWork.GetRepository<Certificate>()
            .SingleOrDefaultAsync(predicate:x => x.ReportNumber == createDiamondRequest.ReportNumber);
        if (certificate is not null)
        {
            throw new BadRequestException("Certificate is already existed");
        }
        var newCertificate = createDiamondRequest.Adapt<Certificate>();
        newCertificate.DateOfIssue = DateTime.Now.AddMonths(createDiamondRequest.WarrantyPeriod);
        await _unitOfWork.GetRepository<Certificate>().InsertAsync(newCertificate);
        await _unitOfWork.CommitAsync();
        var diamond = createDiamondRequest.Adapt<Diamond>();
        diamond.CertificateId = newCertificate.Id;
        await _unitOfWork.GetRepository<Diamond>().InsertAsync(diamond);
        await _unitOfWork.CommitAsync();
        if (createDiamondRequest.DiamondImages is not [])
        {
            await _pictureService.UploadDiamondPictures(createDiamondRequest.DiamondImages, diamond.Id);
        }

        return new GetDiamondId
        {
            Id = diamond.Id
        };
    }

    public async Task UpdateDiamond(Guid id, UpdateDiamondRequest updateDiamondRequest)
    {
        var diamond = await _unitOfWork.GetRepository<Diamond>()
            .SingleOrDefaultAsync( predicate: p => p.Id == id
                                    ,include: p => p.Include(x => x.Certificate)
                                                                     .Include(x => x.Pictures));
        var certificate = await _unitOfWork.GetRepository<Certificate>()
            .SingleOrDefaultAsync(predicate: x => x.Id == diamond.CertificateId);
        if (diamond is null)
        {
            throw new NotFoundException($"Can't find any diamonds with id {id}");
        }
        var existingCertificate = await _unitOfWork.GetRepository<Certificate>()
            .SingleOrDefaultAsync(predicate:x => x.ReportNumber == updateDiamondRequest.ReportNumber);
        if (existingCertificate is not null && existingCertificate.Id != diamond.CertificateId)
        {
            throw new BadRequestException("Another certificate with the same report number already exists");
        }
        updateDiamondRequest.Adapt(certificate);
        updateDiamondRequest.Adapt(diamond);
        diamond.LastUpdate = DateTime.Now;
        if (diamond.Pictures.Any())
        {
            await _pictureService.DeletePictures(diamond.Pictures);
            
            diamond.Pictures.Clear();
        }
        await _unitOfWork.CommitAsync();
        if (updateDiamondRequest.DiamondImages is not [])
        {
            await _pictureService.UploadDiamondPictures(updateDiamondRequest.DiamondImages, diamond.Id);
        }
        
    }

    public async Task<Paginate<GetDiamondPagedResponse>> GetPageDiamonds(int page, int size)
    {
        var diamonds = await _unitOfWork.GetRepository<Diamond>().GetPagingListAsync(include: x => x.Include(p => p.Pictures).Include(p => p.Certificate), page: page, size: size);
        return diamonds.Adapt<Paginate<GetDiamondPagedResponse>>();
    }

    public async Task<GetDiamondPagedResponse> GetDiamondDetailsById(Guid id)
    {
        var diamond = await _unitOfWork.GetRepository<Diamond>().SingleOrDefaultAsync(include: x => x.Include(p => p.Pictures).Include(p => p.Certificate), predicate: p => p.Id == id);
        if (diamond is null)
        {
            throw new NotFoundException($"Can't find any diamonds with id {id}");
        }
        return diamond.Adapt<GetDiamondPagedResponse>();
    }

    public async Task ChangStatusDiamond(Guid diamondId, DiamondStatus status)
    {
        var diamond = await _unitOfWork.GetRepository<Diamond>().SingleOrDefaultAsync(predicate: p => p.Id == diamondId,
            include: x => x.Include(p => p.Certificate));
        if (diamond is null)
        {
            throw new NotFoundException("Diamond not found");
        }
        diamond.Status = status switch
        {
            DiamondStatus.Available => DiamondStatus.Available.ToString().ToLower(),
            DiamondStatus.OutOfStock => DiamondStatus.OutOfStock.ToString().ToLower(),
            DiamondStatus.Unavailable => DiamondStatus.Unavailable.ToString().ToLower(),
            _ => diamond.Status
        };
        diamond.LastUpdate = DateTime.Now;
        diamond.Certificate.Status = status switch
        {
            DiamondStatus.Available => CertificateStatus.Available.ToString().ToLower(),
            DiamondStatus.Unavailable => CertificateStatus.Unavailable.ToString().ToLower(),
            _ => diamond.Certificate.Status
        };
        await _unitOfWork.CommitAsync();
    }
}