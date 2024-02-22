using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.DTO
{
    public class CusView
    {
        public int AccountID { get; set; }
        public string Email { get; set; }

        public string FullName { get; set; }
        public string Phone { get; set; }

        public string Address { get; set; }
        public bool Admin {  get; set; }
    }
}
