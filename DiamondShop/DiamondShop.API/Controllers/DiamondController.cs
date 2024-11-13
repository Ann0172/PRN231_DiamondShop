using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Diamond;
using DiamondShop.Repository.ViewModels.Response.Diamond;
using DiamondShop.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShop.API.Controllers
{
    [Route("api/diamonds")]
    [ApiController]
    public class DiamondController : ControllerBase
    {
        private readonly IDiamondService _diamondService;

        public DiamondController(IDiamondService diamondService)
        {
            _diamondService = diamondService;
        }

        [HttpGet]
        public async Task<ActionResult<Paginate<GetDiamondPagedResponse>>> GetPagedDiamonds(int page = 1, int size = 10)
        {
            return await _diamondService.GetPageDiamonds(page, size);
        } 

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetDiamondPagedResponse>> GetDiamondDetailsById(Guid id)
        {
            return await _diamondService.GetDiamondDetailsById(id);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateDiamond([FromForm] CreateDiamondRequest createDiamondRequest)
        {
            return Created(nameof(CreateDiamond), await _diamondService.CreateDiamond(createDiamondRequest));
        }
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateDiamond([FromRoute] Guid id, [FromForm] UpdateDiamondRequest updateDiamondRequest)
        {
            await _diamondService.UpdateDiamond(id, updateDiamondRequest);
            return NoContent();
        }
        [HttpPatch("{diamondId:guid}/{status}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeStatusDiamond(Guid diamondId, DiamondStatus status)
        {
            await _diamondService.ChangStatusDiamond(diamondId, status);
            return NoContent();
        }
    }
}
