using InvacoCase.Data;
using InvacoCase.Interfaces;
using InvacoCase.Models;
using Microsoft.AspNetCore.Identity;

namespace InvacoCase.Business
{
    public class RegisterService : IRegisterServices
    {

        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RegisterService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<User> Register(User user, IFormFile image)
        {
           
            user.ImageUrl = await SaveImage(image);
            var result = _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

		public async Task<IdentityResult> RegisterUser(RegisterViewModel model, UserManager<User> _userManager, IFormFile image)
		{
			var ImageUrl = await SaveImage(image);
			var user = new User { UserName = model.Username, ImageUrl = ImageUrl, Email=model.Email };
			return await _userManager.CreateAsync(user, model.Password);
		}

	
        private async Task<string> SaveImage(IFormFile image)
        {
			if (image == null || image.Length == 0)
			{
				throw new Exception("File not found");
			}

			var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				image.CopyTo(stream);
			}

			return "/images/" + fileName;
        }
    }
}
