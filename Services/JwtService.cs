using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Vessel_Tracking_Api.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Generates a JSON Web Token (JWT) for a given username with a specified expiry time.
        /// </summary>
        /// <remarks>
        /// The token is signed using the <c>JwtSecret</c> from the configuration.
        /// The token contains claims:
        /// <list type="bullet">
        /// <item><c>ClaimTypes.Name</c> - the username</item>
        /// <item><c>sub</c> - the username</item>
        /// <item><c>jti</c> - a unique identifier for the token</item>
        /// </list>
        /// The token will expire after the number of minutes specified by <paramref name="tokenExpiryMinutes"/>.
        /// </remarks>
        /// <param name="username">The username for which the JWT is generated.</param>
        /// <param name="tokenExpiryMinutes">The token expiry duration in minutes.</param>
        /// <returns>A signed JWT as a string, which can be used for authentication in API requests.</retur
        
        public string GenerateToken(string username, int tokenExpiryMinutes)
        {
            var securitySection = _config.GetSection("ApiSecurity");

            // Create the security key from config
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(securitySection["JwtSecret"])
            );

            // Create signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Generate token with dynamic expiry
            var token = new JwtSecurityToken(
                issuer: securitySection["Issuer"],
                audience: securitySection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpiryMinutes), // <-- use the parameter
                signingCredentials: creds
            );

            // Return the JWT as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}