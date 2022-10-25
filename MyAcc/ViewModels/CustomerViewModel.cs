using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }                
        public string Fullname { get; set; }                        
        public string Billingaddress { get; set; }
        public string Deliveryaddress { get; set; }
    }
}
