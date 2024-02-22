using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.DTO
{
    public class OrderAdminResponse
    {
        public int OrderID { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime ShipperDate { get; set; }
        public DateTime OrderDate { get; set; }
        public double OrderQuantity { get; set; }
        public string OrderNote { get; set; }

    }
}
