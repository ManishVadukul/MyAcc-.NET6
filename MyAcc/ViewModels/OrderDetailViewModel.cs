using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.ViewModels
{
    public class OrderDetailViewModel
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }                
        public decimal? Cost { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public int Tax { get; set; }        
        public decimal? VAT { get; set; }
        public decimal? Total { get; set; }
    }
}
