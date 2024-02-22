using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.DTO
{
    public class CartResponse
    {
        public int CartId { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public string ProductName { get; set; }
        public int CartQuantity { get; set; }
        public string ProductImage { get; set; }
        public double Price { get; set; }
        
    }
}
