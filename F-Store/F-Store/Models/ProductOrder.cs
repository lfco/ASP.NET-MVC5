﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace F_Store.Models
{
    public class ProductOrder: Product
    {

        //[DisplayFormat(DataFormatString = "{0:N2", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "You must enter the field {0}")]
        public float Quantity { get; set; }
       
        [DataType(DataType.Currency)]
        //[DisplayFormat(DataFormatString = "{0:C2", ApplyFormatInEditMode = false)]
        public decimal Value { get { return Price * (decimal)Quantity; } }
    }
}