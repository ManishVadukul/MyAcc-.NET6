using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MyAcc.Models
{
    public class TaxType
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Perce { get; set; }      

    }
}
