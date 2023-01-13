using InvacoCase.Models;
using InvacoCase.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InvacoCase.Interfaces
{
    public interface ILoginService
    {
        public Task<string> Login(string email, string password);
        public Task<string> LoginUser(LoginViewModel model, SignInManager<User> _signInManager);

	}
}
