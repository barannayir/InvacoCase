using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InvacoCase.Models
{
    public class User : IdentityUser<int>
	{
       
        public string ImageUrl { get; set; }
    }
}
