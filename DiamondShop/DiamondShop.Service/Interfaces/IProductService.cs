using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Product;
using DiamondShop.Repository.ViewModels.Response.Product;

namespace DiamondShop.Service.Interfaces;

public interface IProductService
{
    Task<GetProductIdResponse> CreateProduct(CreateProductRequest createProductRequest);
    Task UpdateProduct(Guid id, UpdateProductRequest updateProductRequest);
    Task<Paginate<GetProductPagedResponse>> GetPagedProducts(QueryProductRequest queryProductRequest, int page, int size);
    Task<GetProductDetailResponse> GetProductDetailById(Guid id);
    Task UpdateProductStatus(Guid productId, ProductStatus status);
}