using Contracts.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Auth;
using Models.Ride;
using System.Security.Claims;
using TaxiWeb.Services;

namespace TaxiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideController : ControllerBase
    {
        private readonly IBussinesLogic authService;
        private readonly IRequestAuth requestAuth;
        public RideController(IBussinesLogic authService, IRequestAuth requestAuth)
        {
            this.authService = authService;
            this.requestAuth = requestAuth;
        }

        [HttpPost]
        [Authorize]
        [Route("estimate-ride")]
        public async Task<IActionResult> EstimateRide([FromBody] EstimateRideRequest request)
        {
            var userHasRightToAccess = requestAuth.DoesUserHaveRightsToAccessResource(HttpContext, new UserType[] { UserType.CLIENT });
            if(!userHasRightToAccess)
            {
                return Unauthorized();
            }

            return Ok(await authService.EstimateRide(request));
        }

        [HttpPost]
        [Authorize]
        [Route("create-ride")]
        public async Task<IActionResult> CreateRide([FromBody] CreateRideRequest request)
        {
            var userHasRightToAccess = requestAuth.DoesUserHaveRightsToAccessResource(HttpContext, new UserType[] { UserType.CLIENT });
            if (!userHasRightToAccess)
            {
                return Unauthorized();
            }

            var roleId = requestAuth.GetRoleIdFromContext(HttpContext);

            if (roleId == null)
            {
                return BadRequest("Invalid JWT");
            }

            request.Id = Guid.NewGuid();
            var res = await authService.CreateRide(request, (Guid)roleId);

            if (res == null) 
            {
                return BadRequest("Failed to create ride");
            }

            return Ok(res);
        }

        [HttpPatch]
        [Authorize]
        [Route("update-ride-status")]
        public async Task<IActionResult> UpdateRideStatus([FromBody] UpdateRideRequest request)
        {
            var userHasRightToAccess = requestAuth.DoesUserHaveRightsToAccessResource(HttpContext, new UserType[] { UserType.CLIENT, UserType.DRIVER });

            // Client will update once ride is finished, Driver will update when accepting the ride
            if (!userHasRightToAccess)
            {
                return Unauthorized();
            }

            var userType = requestAuth.GetUserTypeFromContext(HttpContext);
            var roleId = requestAuth.GetRoleIdFromContext(HttpContext);

            if (roleId == null || userType == null) 
            {
                return BadRequest("Invalid JWT");
            }

            var validUpdate = 
                (userType == UserType.CLIENT && request.Status == RideStatus.COMPLETED) 
                || (userType == UserType.DRIVER && request.Status == RideStatus.ACCEPTED);

            if (!validUpdate)
            {
                return Unauthorized("Can not update ride with given parameters");
            }

            if(userType == UserType.DRIVER)
            {
                var driverStatus = await authService.GetDriverStatus((Guid)roleId);
                if (driverStatus != Models.UserTypes.DriverStatus.VERIFIED)
                {
                    return Unauthorized($"This driver can not accept rides as he is {driverStatus}");
                }
            }

            return Ok(await authService.UpdateRide(request, (Guid)roleId));
        }

        [HttpGet]
        [Authorize]
        [Route("get-new-rides")]
        public async Task<IActionResult> GetNewRides()
        {
            var userHasRightToAccess = requestAuth.DoesUserHaveRightsToAccessResource(HttpContext, new UserType[] { UserType.DRIVER });
            if (!userHasRightToAccess)
            {
                return Unauthorized();
            }
            
            var roleId = requestAuth.GetRoleIdFromContext(HttpContext);

            if (roleId == null)
            {
                return Unauthorized("Bad user email");
            }

            var driverStatus = await authService.GetDriverStatus((Guid)roleId);

            if(driverStatus != Models.UserTypes.DriverStatus.VERIFIED)
            {
                return Unauthorized($"This driver can not see new rides as he is {driverStatus}");
            }

            return Ok(await authService.GetNewRides());
        }

        [HttpGet]
        [Authorize]
        [Route("get-user-rides")]
        public async Task<IActionResult> GetUserRides()
        {
            var userType = requestAuth.GetUserTypeFromContext(HttpContext);
            var roleId = requestAuth.GetRoleIdFromContext(HttpContext);

            if (roleId == null || userType == null)
            {
                return BadRequest("Invalid JWT");
            }

            return Ok(await authService.GetUsersRides((Guid)roleId, (UserType)userType));
        }

        [HttpGet]
        [Authorize]
        [Route("get-ride/{rideId}")]
        public async Task<IActionResult> GetRideStatus(Guid rideId)
        {
            var userType = requestAuth.GetUserTypeFromContext(HttpContext);
            var roleId = requestAuth.GetRoleIdFromContext(HttpContext);

            if (roleId == null || userType == null)
            {
                return BadRequest("Invalid JWT");
            }

            var ride = await authService.GetRideStatus(rideId);

            if(ride == null)
            {
                return BadRequest("Failed to get ride with those parameters");
            }

            var userIsDriverForRide = userType == UserType.DRIVER && roleId.Equals(ride.DriverId);
            var userIsClientForRide = userType == UserType.CLIENT && roleId.Equals(ride.ClientId);
            var userIsAdmin = userType == UserType.ADMIN;

            if(!userIsClientForRide && !userIsDriverForRide && !userIsAdmin)
            {
                return Unauthorized("You can not see this ride.");
            }

            return Ok(ride);
        }
    }
}
