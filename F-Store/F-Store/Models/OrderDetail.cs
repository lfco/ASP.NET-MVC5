using System.ComponentModel.DataAnnotations;

namespace F_Store.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public float Quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}