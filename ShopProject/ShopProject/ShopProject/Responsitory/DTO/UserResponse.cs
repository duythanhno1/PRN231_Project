using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.DTO
{
    public class UserResponse
    {
      

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
    }
}
