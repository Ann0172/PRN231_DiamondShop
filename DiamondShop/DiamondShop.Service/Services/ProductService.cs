using System.Linq.Expressions;
using DiamondShop.Repository;
using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Interfaces;
using DiamondShop.Repository.Models;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Account;
using DiamondShop.Repository.ViewModels.Request.Product;
using DiamondShop.Repository.ViewModels.Response.Product;
using DiamondShop.Service.Extensions;
using DiamondShop.Service.Interfaces;
using DiamondShop.Shared.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DiamondShop.Service.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork<Prn231DiamondShopContext> _unitOfWork;
    private readonly IPictureService _pictureService;

    public ProductService(IUnitOfWork<Prn231DiamondShopContext> unitOfWork, IPictureService pictureService)
    {
        _unitOfWork = unitOfWork;
        _pictureService = pictureService;
    }
    public async Task<GetProductIdResponse> CreateProduct(CreateProductRequest createProductRequest)
    {
        var category = await _unitOfWork.GetRepository<Category>()
            .SingleOrDefaultAsync(predicate: c => c.Id == createProductRequest.CategoryId);
        if (category is null)
        {
            throw new NotFoundException("Category not found");
        }

        var diamond = await _unitOfWork.GetRepository<Diamond>().SingleOrDefaultAsync(
            predicate: d => d.Id == createProductRequest.DiamondId, include: x => x.Include(p => p.Product!));
        if (diamond is null)
        {
            throw new NotFoundException("Diamond is not existed");
        }

        if (diamond.Product is not null)
        {
            throw new BadRequestException("This Diamond is already linked to a Product");
        }

        var product = createProductRequest.Adapt<Product>();
        await _unitOfWork.GetRepository<Product>().InsertAsync(product);
        await _unitOfWork.CommitAsync();
        if (createProductRequest.ProductPicture is not [])
        {
            await _pictureService.UploadProductPictures(createProductRequest.ProductPicture, product.Id);
        }

        return new GetProductIdResponse { Id = product.Id };
    }

    public async Task UpdateProduct(Guid id, UpdateProductRequest updateProductRequest)
    {
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(predicate: p => p.Id == id);
        if (product is null)
        {
            throw new NotFoundException("Product is not existed");
        }
        var category = await _unitOfWork.GetRepository<Category>()
            .SingleOrDefaultAsync(predicate: c => c.Id == updateProductRequest.CategoryId);
        if (category is null)
        {
            throw new NotFoundException("Category not found");
        }
        var diamond = await _unitOfWork.GetRepository<Diamond>().SingleOrDefaultAsync(
            predicate: d => d.Id == updateProductRequest.DiamondId, include: x => x.Include(p => p.Product!));
        if (diamond is null)
        {
            throw new NotFoundException("Diamond is not existed");
        }
        if (diamond.Product is not null && diamond.Product.Id != id)
        {
            throw new BadRequestException("This Diamond is already linked to another Product");
        }
        updateProductRequest.Adapt(product);
        product.LastUpdate = DateTime.Now;
        if (product.Pictures.Any())
        {
            await _pictureService.DeletePictures(product.Pictures);
            product.Pictures.Clear();
        }
        _unitOfWork.GetRepository<Product>().UpdateAsync(product);
        await _unitOfWork.CommitAsync();
        if (updateProductRequest.ProductPicture is not [])
        {
            await _pictureService.UploadProductPictures(updateProductRequest.ProductPicture, product.Id);
        }
    }

    public async Task<Paginate<GetProductPagedResponse>> GetPagedProducts(QueryProductRequest queryProductRequest, int page, int size)
    {
        
        var products = await _unitOfWork.GetRepository<Product>()
            .GetPagingListAsync(predicate: ApplyProductFilter(queryProductRequest), page: page, size: size,
                include: x => x.Include(p => p.Category).Include(p => p.Pictures).Include(p => p.Diamond));
        return products.Adapt<Paginate<GetProductPagedResponse>>();
    }

    public async Task<GetProductDetailResponse> GetProductDetailById(Guid id)
    {
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(predicate: x => x.Id == id,
            include: x => x.Include(p => p.Category).Include(p => p.Pictures).Include(p => p.Diamond));
        if (product is null)
        {
            throw new NotFoundException("Product not found");
        }
        
        return product.Adapt<GetProductDetailResponse>();
    }

    public async Task UpdateProductStatus(Guid productId, ProductStatus status)
    {
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(predicate: p => p.Id == productId);
        if (product is null)
        {
            throw new NotFoundException("Product not found");
        }

        product.Status = status switch
        {
            ProductStatus.Available => ProductStatus.Available.ToString().ToLower(),
            ProductStatus.OutOfStock => ProductStatus.OutOfStock.ToString().ToLower(),
            ProductStatus.Unavailable => ProductStatus.Unavailable.ToString().ToLower(),
            _ => product.Status
        };

        product.LastUpdate = DateTime.Now;
        _unitOfWork.GetRepository<Product>().UpdateAsync(product);
        await _unitOfWork.CommitAsync();
    }

    private Expression<Func<Product, bool>> ApplyProductFilter(QueryProductRequest? filter)
    {
        if (filter == null) return product => true;

        Expression<Func<Product, bool>> filterQuery = product => true;
        if (filter.StartPrice < filter.EndPrice)
        {
            filterQuery = filterQuery.AndAlso(product => product.Price >= filter.StartPrice && product.Price <= filter.EndPrice);
        }
        if (filter.CategoryIds is not [])
        {
            filterQuery = filterQuery.AndAlso(product => filter.CategoryIds.Contains(product.CategoryId));
        }
        if (!string.IsNullOrEmpty(filter.Material))
        {
            filterQuery = filterQuery.AndAlso(product => !string.IsNullOrEmpty(product.Material) &&
                                                     product.Material.ToLower().Contains(filter.Material.ToLower()));
        }
        if (!string.IsNullOrEmpty(filter.Name))
        {
            filterQuery = filterQuery.AndAlso(product => !string.IsNullOrEmpty(product.Name) &&
                                                     product.Name.ToLower().Contains(filter.Name.ToLower()));
        }
        if (filter.DiamondIds is not [])
        {
            filterQuery = filterQuery.AndAlso(product => filter.DiamondIds.Contains(product.DiamondId));
        }

        return filterQuery;
    }
    
}