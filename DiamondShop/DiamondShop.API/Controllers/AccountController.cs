using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Account;
using DiamondShop.Repository.ViewModels.Response.Account;
using DiamondShop.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShop.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Paginate<GetAccountDetailResponse>>> GetPagingAccount([FromQuery] QueryAccountRequest queryAccountRequest, [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            return await _accountService.GetPagedAccount(queryAccountRequest, page, size);
        }
    }
}
