using InvacoCase.Data;
using InvacoCase.Interfaces;
using InvacoCase.Models;
using InvacoCase.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InvacoCase.Business
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly UserManager<User> _userManager;
		public ProfileService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<IdentityResult> UpdateProfile(ProfileViewModel model, IFormFile image)
           {
			var user = await _userManager.FindByNameAsync(model.Username);
			if (model.Email != null)
			{
				user.Email = model.Email;
			}
			if (model.Password != null)
			{
                await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
			}
			if (image != null)
			{
				user.ImageUrl = await SaveImage(image);
			}
            var result = await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();
            return result;
        }
        public async Task<string> SaveImage(IFormFile image)
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
