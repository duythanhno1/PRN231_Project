using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "OrderDetailID is required")]
        public int OrderDetailID { get; set; }

        [Required(ErrorMessage = "OrderDetailTotalPrice is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "OrderDetailTotalPrice must be greater than 0")]
        public double OrderDetailTotalPrice { get; set; }
        [Required(ErrorMessage = "OrderDetailTotalPrice is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "OrderDetailTotalPrice must be greater than 0")]
        public int OrderDetailQuantity { get; set; }

        [Required(ErrorMessage = "OrderDetailStatus is required")]
        public string OrderDetailStatus { get; set; }

        [ForeignKey("Order")]
        [Required(ErrorMessage = "OrderID is required")]
        public int OrderID { get; set; }
        [ForeignKey("Product")]
        [Required(ErrorMessage = "ProductID is required")]
        public int ProductID { get; set; }
        [ForeignKey("User")]
        [Required(ErrorMessage = "UserID is required")]
        public int UserID { get; set; }
    }
}
