using Contracts.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.Auth;
using Models.UserTypes;
using System.Security.Claims;
using TaxiWeb.Services;
using static TaxiWeb.Controllers.DriverController;

namespace TaxiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IBussinesLogic authService;
        private readonly IRequestAuth requestAuth;
        public DriverController(IBussinesLogic authService, IRequestAuth requestAuth)
        {
            this.authService = authService;
            this.requestAuth = requestAuth;
        }

        [HttpGet]
        [Authorize]
        [Route("driver-status/{driverId}")]
        public async Task<IActionResult> GetDriverStatus(Guid driverId)
        {
            bool userCanAccessResource = requestAuth.DoesUserHaveRightsToAccessResource(HttpContext, new UserType[] { UserType.ADMIN, UserType.DRIVER });
            if (!userCanAccessResource)
            {
                return Unauthorized();
            }

            var driverStatus = await authService.GetDriverStatus(driverId);

            return Ok(driverStatus);
        }

        [HttpPatch]
        [Authorize]
        [Route("driver-status/{driverId}")]
        public async Task<IActionResult> UpdateDriverStatus(Guid driverId, [FromBody] UpdateDriverStatusData updateData)
        {
            bool userCanAccessResource = requestAuth.DoesUserHaveRightsToAccessResource(HttpContext, new UserType[] { UserType.ADMIN });
            var roleId = requestAuth.GetRoleIdFromContext(HttpContext);

            if (!userCanAccessResource || roleId == null)
            {
                return Unauthorized();
            }

            var result = await authService.UpdateDriverStatus(driverId, updateData.Status);

            if (result)
            {
                await authService.SendEmail(new Models.Email.SendEmailRequest()
                {
                    Body = $"Your status on TaxiWeb application has been changed to {updateData.Status.ToString()}",
                    // TO DO: Change to driver's email
                    EmailTo = "lukaivka24@gmail.com",
                    Subject = "TaxiWeb status update"
                });
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [Route("list-drivers")]
        public async Task<IActionResult> ListAllDrivers()
        {
            bool userCanAccessResource = requestAuth.DoesUserHaveRightsToAccessResource(HttpContext, new UserType[] { UserType.ADMIN });
            if (!userCanAccessResource)
            {
                return Unauthorized();
            }

            return Ok(await authService.ListAllDrivers());
        }

        [HttpPost]
        [Authorize]
        [Route("rate-driver")]
        public async Task<IActionResult> RateDriver([FromBody] RideRating driverRating)
        {
            bool userCanAccessResource = requestAuth.DoesUserHaveRightsToAccessResource(HttpContext, new UserType[] { UserType.CLIENT });
            if (!userCanAccessResource)
            {
                return Unauthorized();
            }
            driverRating.Id = Guid.NewGuid();
            return Ok(await authService.RateDriver(driverRating));
        }

        [HttpPost]
        [Authorize]
        [Route("avg-rating-driver/{driverId}")]
        public async Task<IActionResult> AverageRatingDriver(Guid driverId)
        {
            bool userCanAccessResource = requestAuth.DoesUserHaveRightsToAccessResource(HttpContext, new UserType[] { UserType.ADMIN });
            var roleId = requestAuth.GetRoleIdFromContext(HttpContext);

            if (!userCanAccessResource || roleId == null)
            {
                return Unauthorized();
            }

            return Ok(await authService.GetAverageRatingForDriver(driverId));
        }
    }
}
