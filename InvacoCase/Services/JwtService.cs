using InvacoCase.Interfaces;
using InvacoCase.Models;
using InvacoCase.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InvacoCase.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly JwtOptions _options;

        public JwtService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateJwtToken(User user)
        {

			var userName = user.UserName.ToString();
			var userId = user.Id.ToString();
			var ImageUrl = user.ImageUrl.ToString();

			var claims = new[]
            {
                new Claim("sub", userId),
                new Claim("name", userName),
                new Claim("image_url", ImageUrl)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_options.ExpirationInMinutes),
                signingCredentials: credentials
            );

            return _jwtSecurityTokenHandler.WriteToken(token);
        }

		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
				ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken securityToken;
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
			var jwtSecurityToken = securityToken as JwtSecurityToken;
			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				throw new SecurityTokenException("Invalid token");

			return principal;
		}

	}
}
