using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "OrderID is required")]
        public int OrderID { get; set; }
        [Required(ErrorMessage = "OrderID is required")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "OrderDate is required")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "OrderQuantity is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "OrderQuantity must be greater than 0")]
        public double OrderQuantity { get; set; }

        [Required(ErrorMessage = "ShipperDate is required")]
        public DateTime ShipperDate { get; set; }

        [Required(ErrorMessage = "OrderNote is required")]
        public string OrderNote { get; set; }

        [Required(ErrorMessage = "OrderStatus is required")]
        public string OrderStatus { get; set; }
    }
}
