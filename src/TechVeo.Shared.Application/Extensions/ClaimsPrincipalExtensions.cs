using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace TechVeo.Shared.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var claimValue = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (claimValue != null)
        {
            if (Guid.TryParse(claimValue, out var userId))
            {
                return userId;
            }
        }

        throw new Exception("The Sub is not present in claims.");
    }
}
