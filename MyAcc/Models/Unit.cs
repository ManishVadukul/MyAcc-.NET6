using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.Models
{
    public class Unit
    {
        [Required]
        public int UnitId { get; set; }
        public string UnitName { get; set; }
    }
}
