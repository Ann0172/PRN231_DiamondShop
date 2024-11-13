using System.ComponentModel.DataAnnotations;
using DiamondShop.Repository.Enums;
using Microsoft.AspNetCore.Http;

namespace DiamondShop.Repository.ViewModels.Request.Account;

public class UpdateAccountRequest
{
    public required string Username { get; set; } 

    public required string PhoneNumber { get; set; }

    public IFormFile? Avatar { get; set; }
}