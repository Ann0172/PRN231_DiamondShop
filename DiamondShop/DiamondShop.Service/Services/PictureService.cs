using DiamondShop.Repository;
using DiamondShop.Repository.Interfaces;
using DiamondShop.Repository.Models;
using DiamondShop.Service.Interfaces;
using DiamondShop.Shared.Exceptions;
using Microsoft.AspNetCore.Http;

namespace DiamondShop.Service.Services;

public class PictureService : IPictureService
{
    private readonly IUnitOfWork<Prn231DiamondShopContext> _unitOfWork;
    private readonly IFirebaseService _firebaseService;

    public PictureService(IUnitOfWork<Prn231DiamondShopContext> unitOfWork, IFirebaseService firebaseService)
    {
        _unitOfWork = unitOfWork;
        _firebaseService = firebaseService;
    }
    public async Task UploadDiamondPictures(List<IFormFile> pictureFiles, Guid diamondId)
    {
        if (pictureFiles is [])
        {
            throw new BadRequestException("No picture files found");
        }
        var pictureUrls = await _firebaseService.UploadImagesAsync(pictureFiles);
        List<Picture> pictures = [];

        foreach (var url in pictureUrls)
        {
            pictures.Add(new Picture
            {
                DiamondId = diamondId,
                UrlPath = url
            });
        }
        await _unitOfWork.GetRepository<Picture>().InsertRangeAsync(pictures);
        await _unitOfWork.CommitAsync();
    }

    public async Task UploadProductPictures(List<IFormFile> pictureFiles, Guid productId)
    {
        if (pictureFiles is [])
        {
            throw new BadRequestException("No picture files found");
        }

        var pictureUrls = await _firebaseService.UploadImagesAsync(pictureFiles);

        List<Picture> pictures = [];

        foreach (var url in pictureUrls)
        {
            pictures.Add(new Picture
            {
                ProductId = productId,
                UrlPath = url
            });
        }
        await _unitOfWork.GetRepository<Picture>().InsertRangeAsync(pictures);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeletePictures(IEnumerable<Picture> pictures)
    {
        var pictureUrls = pictures.Select(p => p.UrlPath);

        await _firebaseService.DeleteImagesAsync(pictureUrls.ToList());
        _unitOfWork.GetRepository<Picture>().DeleteRangeAsync(pictures);

        await _unitOfWork.CommitAsync();
    }
}