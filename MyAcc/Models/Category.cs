using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MyAcc.Models
{
    public  class Category
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }     

    }
}
