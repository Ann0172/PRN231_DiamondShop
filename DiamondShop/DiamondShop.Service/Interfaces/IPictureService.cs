using DiamondShop.Repository.Models;
using Microsoft.AspNetCore.Http;

namespace DiamondShop.Service.Interfaces;

public interface IPictureService
{
    Task UploadDiamondPictures(List<IFormFile> pictureFiles, Guid diamondId);
    Task UploadProductPictures(List<IFormFile> pictureFiles, Guid productId);
    Task DeletePictures(IEnumerable<Picture> pictures);
}