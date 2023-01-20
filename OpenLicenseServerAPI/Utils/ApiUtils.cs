using System.Security.Claims;

namespace OpenLicenseServerAPI.Utils;

public static class ApiUtils
{
    public const string UserIdClaimType = "UserId";
    
    public static int? GetCurrentUserId(ClaimsPrincipal user)
    {
        return int.TryParse(user.FindFirstValue(UserIdClaimType), out var userId) ? userId : null;
    }
    
    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        return user.IsInRole("Admin");
    }
}