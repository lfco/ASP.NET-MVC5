using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace F_Store.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Display(Name = "Category Description")]
        [Required(ErrorMessage = "You must enter {0}")]
        public string Description { get; set; }
    }
}