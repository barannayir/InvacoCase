using InvacoCase.Models;
using InvacoCase.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InvacoCase.Interfaces
{
    public interface IProfileService
    {
        public Task<IdentityResult> UpdateProfile(ProfileViewModel model, IFormFile image);
        public Task<string> SaveImage(IFormFile image);
    }
}
