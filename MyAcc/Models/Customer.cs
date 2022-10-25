using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyAcc.Models
{
    public class Customer
    {
      
        [Required]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string FullName { get; set; }                
        public string BusinessName { get; set; }
        [DisplayName("Customer Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Billingaddress { get; set; }        
        public string Deliveryaddress { get; set; }

        public string Mobile { get; set; }
        public string HomeNo { get; set; }


    }
}
