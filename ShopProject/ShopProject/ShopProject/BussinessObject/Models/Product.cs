using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "ProductID is required")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "ProductName is required")]
        [MaxLength(255, ErrorMessage = "Product name must be at most 255 characters")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "ProductImage is required")]
        public string ProductImage { get; set; }

        [Required(ErrorMessage = "ProductPrice is required")]
        public double ProductPrice { get; set; }

        [ForeignKey("Category")]
        [Required(ErrorMessage = "CategoryID is required")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "ProductQuantity is required")]
        public int ProductQuantity { get; set; }

        [Required(ErrorMessage = "ProductDetailDescription is required")]
        public string ProductDetailDescription { get; set; }

        [Required(ErrorMessage = "ProductChipset is required")]
        public string ProductChipset { get; set; }

        [Required(ErrorMessage = "ProductStorageInternal is required")]
        public string ProductStorageInternal { get; set; }

        [Required(ErrorMessage = "ProductStorageExternal is required")]
        public string ProductStorageExternal { get; set; }

        [Required(ErrorMessage = "ProductBatteryCapacity is required")]
        public int ProductBatteryCapacity { get; set; }

        [Required(ErrorMessage = "ProductSelfieCamera is required")]
        public string ProductSelfieCamera { get; set; }

        [Required(ErrorMessage = "ProductMainCamera is required")]
        public string ProductMainCamera { get; set; }

        [MaxLength(50, ErrorMessage = "ProductStatus must be at most 50 characters")]
        [Required(ErrorMessage = "ProductStatus is required")]
        public string ProductStatus { get; set; }
    }
}
