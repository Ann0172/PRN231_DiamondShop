using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DiamondShop.Repository;
using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Interfaces;
using DiamondShop.Repository.Models;
using DiamondShop.Repository.ViewModels.Request.Account;
using DiamondShop.Repository.ViewModels.Request.Auth;
using DiamondShop.Repository.ViewModels.Response.Account;
using DiamondShop.Repository.ViewModels.Response.Auth;
using DiamondShop.Service.Extensions;
using DiamondShop.Service.Interfaces;
using DiamondShop.Shared.Exceptions;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DiamondShop.Service.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork<Prn231DiamondShopContext> _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IFirebaseService _firebaseService;

    public AuthService(IUnitOfWork<Prn231DiamondShopContext> unitOfWork, IConfiguration configuration, IFirebaseService firebaseService)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _firebaseService = firebaseService;
    }
    public async Task Register(RegisterRequest registerRequest)
    {
        var accountDb = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate:x => x.Email == registerRequest.Email);
        if (accountDb is not null)
        {
            throw new BadRequestException("Email already exists");
        }

        registerRequest.Password = HashPassword(registerRequest.Password);
        var account = registerRequest.Adapt<Account>();
        account.Role = Role.Customer.ToString();
        await _unitOfWork.GetRepository<Account>().InsertAsync(account);
        await _unitOfWork.CommitAsync();
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest)
    {
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate:a => a.Email == loginRequest.Email
            && a.Password == HashPassword(loginRequest.Password));
        if (account is null)
        {
            throw new UnauthorizedException("Wrong email or password");
        }

        return GenerateAccessToken(account.Id, account.Role);
    }

    public async Task<GetAccountDetailResponse> GetCurrentAccount(ClaimsPrincipal claims)
    {
        var accountId = claims.GetAccountId();

        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate:a => a.Id == accountId);

        if (account is null)
        {
            throw new UnauthorizedException("Unauthorized");
        }

        return account.Adapt<GetAccountDetailResponse>();
    }

    public async Task<GetAccountDetailResponse> CreateAccount(CreateAccountRequest createAccountRequest)
    {
        var accountDb = await _unitOfWork.GetRepository<Account>()
            .SingleOrDefaultAsync(predicate: a => a.Email == createAccountRequest.Email);
        if (accountDb is not null)
        {
            throw new BadRequestException("Email is already existed");
        }

        var account = createAccountRequest.Adapt<Account>();
        account.Password = HashPassword(createAccountRequest.Password);
        if (createAccountRequest.Avatar != null)
        {
            account.AvatarUrl = await _firebaseService.UploadImageAsync(createAccountRequest.Avatar);
        }

        await _unitOfWork.GetRepository<Account>().InsertAsync(account);
        await _unitOfWork.CommitAsync();
        return account.Adapt<GetAccountDetailResponse>();
    }

    public async Task UpdateAccount(ClaimsPrincipal claim, UpdateAccountRequest updateAccountRequest)
    {
        var accountId = claim.GetAccountId();
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: a => a.Id == accountId);
        updateAccountRequest.Adapt(account);
        if (account.AvatarUrl != null)
        {
            await _firebaseService.DeleteImageAsync(account.AvatarUrl);
            account.AvatarUrl = null;
        }

        if (updateAccountRequest.Avatar != null)
        {
            account.AvatarUrl = await _firebaseService.UploadImageAsync(updateAccountRequest.Avatar);
        } 
        _unitOfWork.GetRepository<Account>().UpdateAsync(account);
        await _unitOfWork.CommitAsync();
    }

    private LoginResponse GenerateAccessToken(Guid accountId, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtAuth:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>(){
            new(ClaimTypes.Role, role),
            new("aid", accountId.ToString())
        };

        var expireTime = DateTime.Now.AddDays(7);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtAuth:Issuer"],
            audience: _configuration["JwtAuth:Audience"],
            claims: claims,
            expires: expireTime,
            signingCredentials: credentials
        );

        return new LoginResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpireIn = expireTime
        };
    }
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();

        // Convert the password string to bytes
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        // Compute the hash
        byte[] hashBytes = sha256.ComputeHash(passwordBytes);

        // Convert the hash to a hexadecimal string
        string hashedPassword = string.Concat(hashBytes.Select(b => $"{b:x2}"));

        return hashedPassword;
    }
}