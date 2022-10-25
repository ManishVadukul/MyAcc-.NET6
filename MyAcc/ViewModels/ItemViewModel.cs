using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.ViewModels
{
    public class ItemViewModel
    {
        public int ProductId { get; set; }        
        public string Code { get; set; }               
        public string Title { get; set; }        
        public decimal Price { get; set; }        
        public decimal Cost { get; set; }        
        public int TaxId { get; set; }
        public int Tax { get; set; }

        public int UnitId { get; set; }
        public string Unitname { get; set; }
        public int? Weight { get; set; }
        
    }
}
