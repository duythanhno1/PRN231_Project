using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "CartId is required")]
        public int CartId { get; set; }

        [Required(ErrorMessage = "ProductID is required")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "ProductID is required")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "ProductName is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "CartQuantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CartQuantity must be greater than 0")]
        public int CartQuantity { get; set; }

        [Required(ErrorMessage = "ProductImage is required")]
        public string ProductImage { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }

        [Required(ErrorMessage = "CartStatus is required")]
        public string CartStatus { get; set; }


    }
}
