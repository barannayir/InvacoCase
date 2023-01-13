using InvacoCase.Models;
using Microsoft.AspNetCore.Identity;

namespace InvacoCase.Interfaces
{
    public interface IRegisterServices
    {
        public Task<User> Register(User user, IFormFile image);
		public Task<IdentityResult> RegisterUser(RegisterViewModel model, UserManager<User> _userManager, IFormFile image);

	}
}
