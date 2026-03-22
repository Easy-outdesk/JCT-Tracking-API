using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Vessel_Tracking_Api.Models;
using Vessel_Tracking_Api.Services;

namespace Vessel_Tracking_Api.Controllers
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly JwtService _jwtService;

        public AuthController(IConfiguration config, JwtService jwtService)
        {
            _config = config;
            _jwtService = jwtService;
        }


        /// <summary>
        /// Generates a JWT token for a valid API user.
        /// </summary>
        /// <remarks>
        /// This endpoint requires the client to send a valid username and password in the request body.
        /// The API key should be sent via the header if rate limiting or security is enforced.
        /// The returned token is valid for the number of minutes specified in <c>ApiSecurity:TokenExpiryMinutes</c> in the configuration.
        /// </remarks>
        /// <param name="request">The user's login credentials (username and password).</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing:
        /// <list type="bullet">
        /// <item><c>access_token</c> - The JWT token string.</item>
        /// <item><c>token_type</c> - Always "Bearer".</item>
        /// <item><c>expires_in</c> - Token lifetime in seconds.</item>
        /// </list>
        /// If credentials are invalid, returns <c>401 Unauthorized</c>.
        /// </returns>
        /// <response code="200">Token successfully generated.</response>
        /// <response code="401">Invalid username or password.</response>
        
        [HttpPost("token")]
        [EnableRateLimiting("ApiPolicy")]
        [HttpPost]
        public IActionResult GetToken([FromBody] TokenRequest request)
        {
            // Get the list of valid API users from configuration
            var users = _config.GetSection("ApiUsers").Get<List<ApiUser>>();

            var user = users.FirstOrDefault(u =>
                u.Username == request.Username &&
                u.Password == request.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            // Read TokenExpiryMinutes from configuration
            int tokenExpiryMinutes = _config.GetValue<int>("ApiSecurity:TokenExpiryMinutes");

            // Generate the JWT token (your service should already use this expiry)
            var token = _jwtService.GenerateToken(user.Username, tokenExpiryMinutes);

            // Return token response
            return Ok(new
            {
                access_token = token,
                token_type = "Bearer",
                expires_in = tokenExpiryMinutes * 60 
            });
        }
    }
}
