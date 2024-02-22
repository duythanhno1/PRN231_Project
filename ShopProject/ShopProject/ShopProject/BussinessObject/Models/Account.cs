using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "AccountID is required")]
        public int AccountID { get; set; }
        [Required(ErrorMessage = "AccountID is required")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "isVerified status is required")]
        public bool isVerified { get; set; }

        [Required(ErrorMessage = "Admin status is required")]
        public bool Admin { get; set; }

        [Required(ErrorMessage = "AccountStatus is required")]
        public string AccountStatus  { get; set; }

    }
}
