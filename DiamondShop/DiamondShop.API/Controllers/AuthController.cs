using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Account;
using DiamondShop.Repository.ViewModels.Request.Auth;
using DiamondShop.Repository.ViewModels.Response.Account;
using DiamondShop.Repository.ViewModels.Response.Auth;
using DiamondShop.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShop.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            await _authService.Register(registerRequest);
            return Ok();
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            return Created(nameof(Login), await _authService.Login(loginRequest));
        }
        [HttpGet("get-current-account")]
        public async Task<ActionResult<GetAccountDetailResponse>> GetCurrentAccount()
        {
            return await _authService.GetCurrentAccount(HttpContext.User);
        }
        
        
    }
    
}
