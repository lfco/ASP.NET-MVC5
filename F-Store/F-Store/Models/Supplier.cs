using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace F_Store.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(30, ErrorMessage = "The field {0} must be between {1} and {2} characters", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DataType(DataType.MultilineText)]
        [StringLength(30, ErrorMessage = "The field {0} must be between {1} and {2} characters", MinimumLength = 3)]
        public string ContactFirstName { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(30, ErrorMessage = "The field {0} must be between {1} and {2} characters", MinimumLength = 3)]
        public string ContactLastName { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(100, ErrorMessage = "The field {0} must be between {1} and {2} characters", MinimumLength = 10)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DataType(DataType.MultilineText)]
        [StringLength(100, ErrorMessage = "The field {0} must be between {1} and {2} characters", MinimumLength = 10)]
        public string Address { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public virtual ICollection<SupplierProduct> SupplierProducts { get; set; }

    }
}