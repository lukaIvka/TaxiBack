using Models.Auth;

namespace TaxiWeb.Services
{
    public interface IRequestAuth
    {
        bool DoesUserHaveRightsToAccessResource(HttpContext httpContext, UserType[] allowedUserTypes);
        string? GetUserEmailFromContext(HttpContext httpContext); 
        UserType? GetUserTypeFromContext(HttpContext httpContext);
        Guid? GetUserIdFromContext(HttpContext httpContext);
        Guid? GetRoleIdFromContext(HttpContext httpContext);
    }
}
