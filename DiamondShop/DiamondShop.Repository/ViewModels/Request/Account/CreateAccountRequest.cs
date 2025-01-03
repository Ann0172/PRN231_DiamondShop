﻿using System.ComponentModel.DataAnnotations;
using DiamondShop.Repository.Enums;
using Microsoft.AspNetCore.Http;

namespace DiamondShop.Repository.ViewModels.Request.Account;

public class CreateAccountRequest
{

    public required string Username { get; set; } 

    public required string Password { get; set; }

    public required string Email { get; set; } 

    public required string PhoneNumber { get; set; }

    public IFormFile? Avatar { get; set; }
    [EnumDataType(typeof(Role))]
    public required string Role { get; set; }
}