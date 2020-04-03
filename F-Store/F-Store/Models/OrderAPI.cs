using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace F_Store.Models
{
    public class OrderAPI
    {
        [Key]
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ICollection<OrderDetail> Details { get; set; }

    }
}