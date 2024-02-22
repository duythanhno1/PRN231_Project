using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.DTO
{
    public class AccountResponse
    {
        public int AccountID { get; set; }
        public int UserID { get; set; }

        public string Email { get; set; }

        public string PassWord { get; set; }

        public bool Admin { get; set; }

    }
}
