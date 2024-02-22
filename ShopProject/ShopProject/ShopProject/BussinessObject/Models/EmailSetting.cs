using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class EmailSetting
    {
        [Required(ErrorMessage = "Mail is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Display Name is required")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Host is required")]
        public string Host { get; set; }

        [Required(ErrorMessage = "Port is required")]
        [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535")]
        public int Port { get; set; }

    }
}
