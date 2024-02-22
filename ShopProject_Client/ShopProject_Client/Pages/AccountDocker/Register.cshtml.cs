using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ShopProject_Client.Pages.AccountDocker
{
	public class RegisterModel : PageModel
	{
		[BindProperty]
		[Required(ErrorMessage = "Fullname is required.")]
		[StringLength(30, MinimumLength = 5, ErrorMessage = "Fullname must be between 5 and 30 characters.")]
		public string Fullname { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Email is required.")]
		[RegularExpression(@"^[\w-]+(\.[\w-]+)*@gmail\.com|[\w-]+(\.[\w-]+)*@fpt\.edu\.vn$", ErrorMessage = "Invalid email format.")]
		public string Email { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Password is required.")]
		[StringLength(16, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 16 characters.")]
		[RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one digit.")]
		public string Password { get; set; }
		[BindProperty]
		[Required(ErrorMessage = "Re_EnterPassword is required.")]
		public string Confirm { get; set; }
		[BindProperty]
		[Required(ErrorMessage = "Address is required.")]
		[StringLength(100, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 100 characters.")]
		public string Address { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Phone is required.")]
		[RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be a 10-digit number.")]
		public string Phone { get; set; }
		public void OnGet()
		{
		}
		public IActionResult OnPost()
		{
			if (ModelState.IsValid)
			{
				var initialData = new
				{
					fullName = Fullname,
					email = Email,
					password = Password,
					address = Address,
					phone = Phone,
					reEnterPassword = Confirm
				};
				TempData["InitialData"] = JsonConvert.SerializeObject(initialData);
				return Page();
			}
			return Page();
		}
	}
}
