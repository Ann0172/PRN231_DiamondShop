using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Category;
using DiamondShop.Repository.ViewModels.Response.Category;
using DiamondShop.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShop.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost]
        public async Task<ActionResult<GetCategoryDetailResponse>> CreateCategory([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            return Created(nameof(CreateCategory), await _categoryService.CreateCategory(createCategoryRequest));
        }
        [HttpGet]
        public async Task<ActionResult<Paginate<GetCategoryDetailResponse>>> GetPagedCategories([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            return await _categoryService.GetPagedCategory(page, size);
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            await _categoryService.UpdateCategory(id, updateCategoryRequest);
            return NoContent();
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetCategoryDetailResponse>> GetCategoryById(Guid id)
        {
            return await _categoryService.GetCategoryById(id);
        }
        [HttpPatch("{id:guid}/{status}")]
        public async Task<ActionResult> ChangeStatusCategory(Guid id, CategoryStatus status)
        {
            await _categoryService.ChangeCategoryStatus(id, status);
            return NoContent();
        }
    }
}
