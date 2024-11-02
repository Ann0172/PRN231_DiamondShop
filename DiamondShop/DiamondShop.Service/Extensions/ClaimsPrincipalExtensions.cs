using System.Security.Claims;
using DiamondShop.Shared.Exceptions;

namespace DiamondShop.Service.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetAccountId(this ClaimsPrincipal claims)
    {
        var accountId = claims.FindFirst(c => c.Type == "aid")?.Value;

        return (accountId is not null) ? Guid.Parse(accountId) : throw new UnauthorizedException("Invalid claims principal");
    }
}