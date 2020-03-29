using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace F_Store.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }
        public int CustomerID { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}