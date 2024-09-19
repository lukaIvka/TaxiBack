using Models.Auth;
using System.Security.Claims;

namespace TaxiWeb.Services
{
    public class RequestAuth : IRequestAuth
    {
        public bool DoesUserHaveRightsToAccessResource(HttpContext httpContext, UserType[] allowedUserTypes)
        {
            var userEmailClaim = httpContext.User.Claims.FirstOrDefault((c) => c.Type == ClaimTypes.Email);
            var userTypeClaim = httpContext.User.Claims.FirstOrDefault((c) => c.Type == ClaimTypes.Role);
            var userIdClaim = httpContext.User.Claims.FirstOrDefault((c) => c.Type == ClaimTypes.NameIdentifier);

            if (userEmailClaim == null || userTypeClaim == null || userIdClaim == null)
            {
                return false;
            }

            var isParsed = Enum.TryParse(userTypeClaim.Value, out UserType userType);

            if (!isParsed)
            {
                return false;
            }

            if (!allowedUserTypes.Contains(userType))
            {
                return false;
            }

            return true;
        }

        public Guid? GetRoleIdFromContext(HttpContext httpContext)
        {
            var roleIdClaim = httpContext.User.Claims.FirstOrDefault((c) => c.Type == ClaimTypes.GroupSid);
            return roleIdClaim != null ? Guid.Parse(roleIdClaim.Value) : null;
        }

        public string? GetUserEmailFromContext(HttpContext httpContext)
        {
            var userEmailClaim = httpContext.User.Claims.FirstOrDefault((c) => c.Type == ClaimTypes.Email);
            return userEmailClaim?.Value;
        }

        public Guid? GetUserIdFromContext(HttpContext httpContext)
        {
            var userIdClaim = httpContext.User.Claims.FirstOrDefault((c) => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : null;
        }

        public UserType? GetUserTypeFromContext(HttpContext httpContext)
        {
            var userTypeClaim = httpContext.User.Claims.FirstOrDefault((c) => c.Type == ClaimTypes.Role);
            if(userTypeClaim == null)
            {
                return null;
            }

            var isParsed = Enum.TryParse(userTypeClaim.Value, out UserType userType);

            if (!isParsed)
            {
                return null;
            }

            return userType;
        }
    }
}
