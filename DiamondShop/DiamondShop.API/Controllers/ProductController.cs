using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Product;
using DiamondShop.Repository.ViewModels.Response.Product;
using DiamondShop.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShop.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost]
        // [Authorize(Roles = "SalesStaff, sales-staff")]
        public async Task<ActionResult> CreateProduct([FromForm] CreateProductRequest createProductRequest)
        {
            return Created(nameof(CreateProduct), await _productService.CreateProduct(createProductRequest));
        }
        [HttpPut("{id:guid}")]
        // [Authorize(Roles = "SalesStaff, sales-staff")]
        public async Task<ActionResult> UpdateProduct([FromRoute] Guid id, [FromForm] UpdateProductRequest updateProductRequest)
        {
            await _productService.UpdateProduct(id, updateProductRequest);
            return NoContent();
        }
        [HttpGet]
        public async Task<ActionResult<Paginate<GetProductPagedResponse>>> GetPagedProducts([FromQuery] QueryProductRequest queryProductRequest, int page = 1, int size = 10)
        {
            return await _productService.GetPagedProducts(queryProductRequest ,page, size);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetProductDetailResponse>> GetProductDetailsById(Guid id)
        {
            return await _productService.GetProductDetailById(id);
        }
        [HttpPatch("{productId:guid}/{status}")]
        public async Task<ActionResult> ChangeStatusProduct(Guid productId, ProductStatus status)
        {
            await _productService.UpdateProductStatus(productId, status);
            return NoContent();
        }
    }
}
