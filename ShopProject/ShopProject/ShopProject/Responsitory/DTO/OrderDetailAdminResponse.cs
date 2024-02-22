using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.DTO
{
    public class OrderDetailAdminResponse
    {
        public int OrderID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime ShipperDate { get; set; }
        public DateTime OrderDate { get; set; }
        public double OrderQuantity { get; set; }
        public string OrderNote { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int ProductQuantity { get; set; }
        public double ProductPrice { get; set; }
        public double OrderDetailTotalPrice { get; set; }
    }
}
