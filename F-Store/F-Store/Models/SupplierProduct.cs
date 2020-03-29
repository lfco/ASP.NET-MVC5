using System.ComponentModel.DataAnnotations;

namespace F_Store.Models
{
    public class SupplierProduct
    {
        [Key]
        public int SupplierProductID { get; set; }
        public int ProductID { get; set; }
        public int SupplierID { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual Product Product { get; set; }
    }
}