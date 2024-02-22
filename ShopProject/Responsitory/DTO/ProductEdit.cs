using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.DTO
{
    public class ProductEdit
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public double ProductPrice { get; set; }
        public int CategoryID { get; set; }
        public int ProductQuantity { get; set; }
        public string ProductDetailDescription { get; set; }
        public string ProductChipset { get; set; }
        public string ProductStorageInternal { get; set; }
        public string ProductStorageExternal { get; set; }
        public int ProductBatteryCapacity { get; set; }
        public string ProductSelfieCamera { get; set; }
        public string ProductMainCamera { get; set; }
    }
}
