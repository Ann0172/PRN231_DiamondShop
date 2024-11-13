using System.Security.Claims;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Account;
using DiamondShop.Repository.ViewModels.Request.Auth;
using DiamondShop.Repository.ViewModels.Response.Account;
using DiamondShop.Repository.ViewModels.Response.Auth;

namespace DiamondShop.Service.Interfaces;

public interface IAuthService
{
    Task Register(RegisterRequest registerRequest);
    Task<LoginResponse> Login(LoginRequest loginRequest);
    Task<GetAccountDetailResponse> GetCurrentAccount(ClaimsPrincipal claims);
    Task<GetAccountDetailResponse> CreateAccount(CreateAccountRequest createAccountRequest);
    Task UpdateAccount(ClaimsPrincipal claim, UpdateAccountRequest updateAccountRequest);
}