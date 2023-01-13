namespace InvacoCase.ViewModels
{
	public class ProfileViewModel
	{
		public string Username { get; set; }
		public string? Email { get; set; }
		public IFormFile? Image { get; set; }
		public string ImageUrl { get; set; }
		public string? Password { get; set; }
		public string? NewPassword { get; set; }
		public string Token { get; set; }
		

	}
}
