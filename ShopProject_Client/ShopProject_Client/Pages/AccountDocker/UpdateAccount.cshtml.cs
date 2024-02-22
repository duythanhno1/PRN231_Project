using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ShopProject_Client.Pages.AccountDocker
{
    public class UpdateAccountModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Fullname is required.")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Fullname must be between 5 and 25 characters.")]
        public string FullName { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Address is required.")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 50 characters.")]
        public string Address { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Phone is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be a 10-digit number.")]
        public string Phone { get; set; }


        public void OnGet()
        {
            var initialData = new
            {
                fullName = FullName,
                phone = Phone,
                address = Address
            };

            TempData["InitialData"] = JsonConvert.SerializeObject(initialData);
        }

        public  IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var initialData = new
                {
                    fullName = FullName,
                    phone = Phone,
                    address = Address
                };

                TempData["InitialData"] = JsonConvert.SerializeObject(initialData);
            }
            return Page();
        }
    }
}
