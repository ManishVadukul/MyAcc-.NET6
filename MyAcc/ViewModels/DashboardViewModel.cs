using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalSales { get; set; }
        public decimal TotalOutstanding { get; set; }
        public int TotalCustomers { get; set; }
    }
}
