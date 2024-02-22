using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.DTO
{
    public class OrderDetailResponse
    {
        public int OrderDetailID { get; set; }
        public double OrderDetailTotalPrice { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
    }
}
