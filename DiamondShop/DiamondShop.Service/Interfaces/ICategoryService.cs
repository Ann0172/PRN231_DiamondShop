using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Category;
using DiamondShop.Repository.ViewModels.Response.Category;

namespace DiamondShop.Service.Interfaces;

public interface ICategoryService
{
    Task<GetCategoryResponse> CreateCategory(CreateCategoryRequest createCategoryRequest);
    Task UpdateCategory(Guid id, UpdateCategoryRequest updateCategoryRequest);
    Task<Paginate<GetCategoryResponse>> GetPagedCategory(int page, int size);
    Task<GetCategoryResponse> GetCategoryById(Guid id);
    Task ChangeCategoryStatus(Guid id, CategoryStatus status);
}