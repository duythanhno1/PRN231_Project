using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ShopProject_Client.Pages.AccountDocker
{
    public class LoginModel : PageModel
    {

        [BindProperty]
        [Required(ErrorMessage = "Email is required.")]
        public string UserName { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            var initialData = new
            {
                username = UserName,
                password = Password
            };

            TempData["InitialData"] = JsonConvert.SerializeObject(initialData);
            return Page();
        }
    }
}
