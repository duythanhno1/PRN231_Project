using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ShopProject_Client.Pages.AccountDocker
{
    public class ChangePasswordModel : PageModel
    {
        [BindProperty]
        [Required]
        public string OldPass { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 16 characters.")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one digit.")]
        public string NewPass { get; set; }
        [BindProperty]
        [Required]
        public string ReNew { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var initialData = new
                {
                    presentPassword = OldPass,
                    newPassword = NewPass,
                    reEnterNewPassword = ReNew
                };

                TempData["InitialData"] = JsonConvert.SerializeObject(initialData);
                return Page();
            }
            return Page();

        }
    }
}
