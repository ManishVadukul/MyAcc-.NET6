using MyAcc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MyAcc.ViewModels
{
    public class OrderOutstanding
    {
        public decimal? FinalTotal { get; set; }
        public decimal? OutstandingTotal { get; set; }
        public decimal? TotalReceived { get; set; }
    }
}
