using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace F_Store.Models
{
    public class DocumentType
    {
        [Key]
        [Display(Name = "Document Type")]
        public int DocumentTypeID { get; set; }

        [Display(Name = "Document")]
        [Required(ErrorMessage = "You must enter {0}")]
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<Employee> Employees { get; set; }
        [JsonIgnore]
        public virtual  ICollection<Customer> Customers { get; set; }
    }
}