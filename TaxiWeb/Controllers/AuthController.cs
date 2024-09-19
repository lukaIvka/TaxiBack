using Contracts.Logic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaxiWeb.ConfigModels;
using TaxiWeb.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaxiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IBussinesLogic authService;
        private readonly IOptions<JWTConfig> jwtConfig;
        private readonly IRequestAuth requestAuth;

        public AuthController(IBussinesLogic authService, IOptions<JWTConfig> jwtConfig, IRequestAuth requestAuth)
        {
            this.authService = authService;
            this.jwtConfig = jwtConfig;
            this.requestAuth = requestAuth;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok("RADI!");
        }
        
        [HttpGet]
        [Authorize]
        [Route("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = requestAuth.GetUserIdFromContext(HttpContext);

            if (userId == null)
            {
                return BadRequest("Invalid JWT");
            }

            return Ok(await authService.GetUserProfile((Guid)userId));
        }

        [HttpPatch]
        [Authorize]
        [Route("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileRequest updateProfileRequest)
        {
            var userId = requestAuth.GetUserIdFromContext(HttpContext);

            if (userId == null)
            {
                return BadRequest("Invalid JWT");
            }

            return Ok(await authService.UpdateUserProfile(updateProfileRequest, (Guid)userId));
        }

        // POST api/<AuthController>/register
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserProfile userProfile)
        {
            try
            {
                userProfile.Id = Guid.NewGuid();
                var res = await authService.Register(userProfile);
                if (res)
                {
                    return new ObjectResult(res) { StatusCode = StatusCodes.Status201Created };
                }

                return new ObjectResult(res) { StatusCode = StatusCodes.Status400BadRequest };
            }
            catch (Exception e)
            {
                return new ObjectResult(e) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginData loginData)
        {
            var existingUser = await authService.Login(loginData);

            if (existingUser == null) 
            {
                return BadRequest();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtConfig.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, loginData.Email), 
                    new Claim(ClaimTypes.Role, existingUser.userType.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, existingUser.userId.ToString()),
                    new Claim(ClaimTypes.GroupSid, existingUser.roleId.ToString())
                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience=jwtConfig.Value.Audience,
                Issuer=jwtConfig.Value.Issuer
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }
    }
}
