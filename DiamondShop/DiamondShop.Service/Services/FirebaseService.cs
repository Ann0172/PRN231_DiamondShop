using DiamondShop.Service.Interfaces;
using DiamondShop.Shared.Exceptions;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DiamondShop.Service.Services;

public class FirebaseService : IFirebaseService
{
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public FirebaseService(StorageClient storageClient, IConfiguration configuration)
        {
            _storageClient = storageClient;
            _bucketName = configuration["Firebase:Bucket"]!;
        }

        private string ExtractImageNameFromUrl(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var segments = uri.Segments;
            var escapedImageName = segments[^1];
            var imageName = Uri.UnescapeDataString(escapedImageName);

            return imageName;
        }

        public async Task DeleteImageAsync(string imageUrl)
        {
            await _storageClient.DeleteObjectAsync(_bucketName, ExtractImageNameFromUrl(imageUrl), cancellationToken: CancellationToken.None);
        }

        public async Task DeleteImagesAsync(List<string> imageUrls)
        {
            var deleteImageTasks = imageUrls.Select(DeleteImageAsync).ToList();

            await Task.WhenAll(deleteImageTasks);
        }

        public string GetImageUrl(string imageName)
        {
            var imageUrl = $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(imageName)}?alt=media";
            return imageUrl;
        }

        public async Task<string> UpdateImageAsync(IFormFile imageFile, string imageName)
        {

            using var stream = new MemoryStream();

            await imageFile.CopyToAsync(stream);

            stream.Position = 0;

            // Re-upload the image with the same name to update it
            var blob = await _storageClient.UploadObjectAsync(_bucketName, imageName, imageFile.ContentType, stream, cancellationToken: CancellationToken.None);

            return GetImageUrl(imageName);
        }


        public async Task<string> UploadImageAsync(IFormFile imageFile, string? imageName = default)
        {

            imageName ??= imageFile.FileName;

            using var stream = new MemoryStream();

            await imageFile.CopyToAsync(stream);

            var blob = await _storageClient.UploadObjectAsync(_bucketName, imageName, imageFile.ContentType, stream, cancellationToken: CancellationToken.None);

            if (blob is null)
            {
                throw new BadRequestException("Upload failed");
            }

            return GetImageUrl(imageFile.FileName);

        }

        public async Task<string[]> UploadImagesAsync(List<IFormFile> imageFiles)
        {

            var uploadTasks = imageFiles.Select(imageFile => UploadImageAsync(imageFile)).ToList();

            var imageUrls = await Task.WhenAll(uploadTasks);

            return imageUrls;
        }
}