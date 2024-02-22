using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.DTO
{
    public class ChangePasswordResponse
    {
        public string PresentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ReEnterNewPassword { get; set; }
    }
}
