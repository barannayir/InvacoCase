using InvacoCase.Business;
using InvacoCase.Interfaces;
using InvacoCase.Models;
using InvacoCase.Services;
using InvacoCase.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InvacoCase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRegisterServices _registerService;
        private readonly ILoginService _loginService;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IJwtService _jwtService;
		private readonly IProfileService _profileService;


		public HomeController(ILogger<HomeController> logger, IRegisterServices registerServices, IJwtService jwtService, ILoginService loginServices, UserManager<User> userManager, SignInManager<User> signInManager, IProfileService profileService)
		{
			_logger = logger;
			_registerService = registerServices;
			_loginService = loginServices;
			_userManager = userManager;
			_signInManager = signInManager;
			_jwtService = jwtService;
			_profileService = profileService;
		}


		public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Profile(string token)
        {
            if (token == null)
            {
                return RedirectToAction("Login");
            }
            var principal = _jwtService.GetPrincipalFromExpiredToken(token);
			var userName = principal.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
			var user = await _userManager.FindByNameAsync(userName);
			if (user == null)
			{
				return Unauthorized();
			}

			var model = new ProfileViewModel
			{
				Username = user.UserName,
				Email = user.Email,
				ImageUrl = user.ImageUrl,
				Token = token
			};

			return View(model);
		}

		public async Task<IActionResult> ProfileUpdate(ProfileViewModel model)
		{
			
				var principal = _jwtService.GetPrincipalFromExpiredToken(model.Token);
				var userName = principal.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
				var user = await _userManager.FindByNameAsync(userName);
				if (user == null)
				{
					return Unauthorized();
				}
				var result = await _profileService.UpdateProfile(model, model.Image);

				if (result.Succeeded)
				{
					return RedirectToAction("Profile","Home", new { token = model.Token });
				}

				return View(model);
			
		}

		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			var token = await _loginService.LoginUser(model, _signInManager);
			if (token == null)
                return BadRequest("Hatalı kullanıcı adı veya şifre");
            return RedirectToAction("Profile", new { token });
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("Register", model);
			}

            var result = _registerService.RegisterUser(model, _userManager, model.Image);
            var loginModel = new LoginViewModel
            {
                Username = model.Username,
                Password = model.Password
            };

            
			if (result.Result.Succeeded)
			{
			var token = await _loginService.LoginUser(loginModel, _signInManager);

                return RedirectToAction("Profile", new { token });

            }
            else
			{
				foreach (var item in result.Result.Errors)
				{
					ModelState.AddModelError("", item.Description);
				}
			}
			return RedirectToAction("Login", "Home");
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}