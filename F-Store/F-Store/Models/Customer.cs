using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F_Store.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Display(Name = "Fist Name")]
        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(30, ErrorMessage = "The field {0} must be between {1} and {2} characters", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(30, ErrorMessage = "The field {0} must be between {1} and {2} characters", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(30, ErrorMessage = "The field {0} must be between {1} and {2} characters", MinimumLength = 3)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DataType(DataType.MultilineText)]
        [StringLength(100, ErrorMessage = "The field {0} must be between {1} and {2} characters", MinimumLength = 10)]
        public string Address { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(20, ErrorMessage = "The field {0} must be between {1} and {2} characters", MinimumLength = 5)]
        public string Document { get; set; }

        public int DocumentTypeID { get; set; }

        [NotMapped]
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } set { } } 

        [JsonIgnore]
        public virtual DocumentType DocumentType { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; }
    }
}