using InvacoCase.Models;
using System.Security.Claims;

namespace InvacoCase.Interfaces
{
    public interface IJwtService
    {
        public string GenerateJwtToken(User user);
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

	}
}
