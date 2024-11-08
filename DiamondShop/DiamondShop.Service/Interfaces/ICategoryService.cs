using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Category;
using DiamondShop.Repository.ViewModels.Response.Category;

namespace DiamondShop.Service.Interfaces;

public interface ICategoryService
{
    Task<GetCategoryDetailResponse> CreateCategory(CreateCategoryRequest createCategoryRequest);
    Task UpdateCategory(Guid id, UpdateCategoryRequest updateCategoryRequest);
    Task<Paginate<GetCategoryDetailResponse>> GetPagedCategory(int page, int size);
    Task<GetCategoryDetailResponse> GetCategoryById(Guid id);
    Task ChangeCategoryStatus(Guid id, CategoryStatus status);
}