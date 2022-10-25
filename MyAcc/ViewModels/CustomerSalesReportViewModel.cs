using Microsoft.AspNetCore.Mvc.Rendering;
using MyAcc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.ViewModels
{
    public class CustomerSalesReportViewModel
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }              
        public decimal? FinalTotal { get; set; }
        public decimal? TotalReceived { get; set; }
        public decimal? TotalRefund { get; set; }
        public decimal? OutstandingTotal { get; set; }
        public decimal TotalVAT { get; set; }
        public string BusinessName { get; set; }        
        public string FullName { get; set; }
        public string Code { get; set; }

        public string startdate { get; set; }
        public string enddate { get; set; }


        public int CustomerId { get; set; }


        public Customer Customer { get; set; }
        public SelectList CustomerList { get; set; } //For Dropdown SelectLitemList


        // Total outstanding Fields

       






    }
}
