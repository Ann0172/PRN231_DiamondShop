using DiamondShop.Repository.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShop.Repository.ViewModels.Request.Account;

public class QueryAccountRequest
{
    [FromQuery(Name = "role")]
    public Role? Role { get; set; }
    [FromQuery(Name = "email")]
    public string? Email { get; set; }
    [FromQuery(Name = "username")]
    public string? UserName { get; set; }
}