using InvacoCase.Data;
using InvacoCase.Interfaces;
using InvacoCase.Models;
using InvacoCase.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InvacoCase.Business
{
    public class LoginService : ILoginService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        public LoginService(SignInManager<User> signInManager, UserManager<User> userManager, ApplicationDbContext context, IJwtService jwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _jwtService = jwtService;
        }
        public async Task<string> Login(string email, string password)
        {
       
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;
          
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                return null;
           
            return _jwtService.GenerateJwtToken(user);
        }

		public async Task<string> LoginUser(LoginViewModel model, SignInManager<User> _signInManager)
        {
			var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, true);
			if (result.Succeeded)
			{
				var user = await _userManager.FindByNameAsync(model.Username);
                var token = _jwtService.GenerateJwtToken(user);
                return token;

			}
			else
			{
                return null;
			}

		}



	}
}
