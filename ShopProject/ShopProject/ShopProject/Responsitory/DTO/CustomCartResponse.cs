using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.DTO
{
    public class CustomCartResponse
    {
        public int CartId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ProductName { get; set; }
        public int CartQuantity { get; set; }
        public string ProductImage { get; set; }
        public double Price { get; set; }
        
    }
}
