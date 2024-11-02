using DiamondShop.Repository;
using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Interfaces;
using DiamondShop.Repository.Models;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Category;
using DiamondShop.Repository.ViewModels.Response.Category;
using DiamondShop.Service.Interfaces;
using DiamondShop.Shared.Exceptions;
using Mapster;

namespace DiamondShop.Service.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork<Prn231DiamondShopContext> _unitOfWork;

    public CategoryService(IUnitOfWork<Prn231DiamondShopContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GetCategoryResponse> CreateCategory(CreateCategoryRequest createCategoryRequest)
    {
        var category = createCategoryRequest.Adapt<Category>();
        await _unitOfWork.GetRepository<Category>().InsertAsync(category);
        await _unitOfWork.CommitAsync();
        return category.Adapt<GetCategoryResponse>();
    }

    public async Task UpdateCategory(Guid id, UpdateCategoryRequest updateCategoryRequest)
    {
        var category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(predicate:x => x.Id == id)
                       ?? throw new NotFoundException("Can't find any category with this id");

        updateCategoryRequest.Adapt(category);
        category.LastUpdate = DateTime.Now;

        await _unitOfWork.CommitAsync();
    }

    public async Task<Paginate<GetCategoryResponse>> GetPagedCategory(int page, int size)
    {
        var categories = await _unitOfWork.GetRepository<Category>().GetPagingListAsync(page:page, size: size);
        return categories.Adapt<Paginate<GetCategoryResponse>>();
    }

    public async Task<GetCategoryResponse> GetCategoryById(Guid id)
    {
        var category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(predicate:x => x.Id == id)
                       ?? throw new NotFoundException("Can't find any category with this id");
        return category.Adapt<GetCategoryResponse>();
    }

    public async Task ChangeCategoryStatus(Guid id, CategoryStatus status)
    {
        var category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(predicate:x => x.Id == id)
                       ?? throw new NotFoundException("Can't find any category with this id");
        category.Status = status switch
        {
            CategoryStatus.Available => CategoryStatus.Available.ToString().ToLower(),
            CategoryStatus.Unavailable => CategoryStatus.Unavailable.ToString().ToLower(),
            _ => category.Status
        };
        category.LastUpdate = DateTime.Now;
        await _unitOfWork.CommitAsync();
    }
}