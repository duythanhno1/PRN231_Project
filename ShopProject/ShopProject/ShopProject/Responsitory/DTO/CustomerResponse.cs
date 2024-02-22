using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.DTO
{
    public class CustomerResponse
    {
 
        public string Email { get; set; }

        public string PassWord { get; set; }
        public string FullName { get; set; }
        public string ReEnterPassword { get; set; }
        public string Phone { get; set; }

        public string Address { get; set; }
    }
}
