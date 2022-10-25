using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyAcc.Models
{
    public class Supplier
    {
     
        
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage ="This field is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        public string Phone { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        public string Address1 { get; set; }               
        public string Address2 { get; set; }

        [Required(ErrorMessage = "This field is required")]        
        public string Town { get; set; }
        public string State { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Postcode { get; set; }
    }
}
