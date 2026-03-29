using JCT_Tracking_Api.Models;
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
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration config, JwtService jwtService, ILogger<AuthController> logger)
        {
            _config = config;
            _jwtService = jwtService;
            _logger = logger;
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
        
        [HttpPost("shipmentAccessKey")]
        [EnableRateLimiting("ApiPolicy")]
        public IActionResult GetToken([FromBody] TokenRequest request)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var requestedUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}";

            _logger.LogInformation(
                "GetToken request received. IP: {IP}, User-Agent: {UA}, URL: {URL}, Username: {Username}",
                ipAddress, userAgent, requestedUrl, request?.Username
            );

            //string hash = BCrypt.Net.BCrypt.HashPassword("Saudi2026@psa%!");
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                _logger.LogWarning("GetToken failed: Username or password not provided. IP: {IP}", ipAddress);
                throw new ValidationException("Username and password are required.");
            }

            var users = _config.GetSection("ApiUsers").Get<List<ApiUser>>();

            var user = users?.FirstOrDefault(u => u.Username == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                _logger.LogWarning("GetToken failed: Invalid credentials for Username: {Username} from IP: {IP}", request.Username, ipAddress);
                throw new UnauthorizedException("Invalid credentials.");
            }

            int tokenExpiryMinutes = _config.GetValue<int>("ApiSecurity:TokenExpiryMinutes");

            var token = _jwtService.GenerateToken(user.Username, tokenExpiryMinutes);

            _logger.LogInformation("Token generated successfully for Username: {Username} from IP: {IP}", request.Username, ipAddress);

            return Ok(new
            {
                success = true,
                message = "Token generated successfully",
                data = new
                {
                    access_token = token,
                    token_type = "Bearer",
                    expires_in = tokenExpiryMinutes * 60
                }
            });
        }
    }
}
