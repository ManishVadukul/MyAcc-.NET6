using MyAcc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }        
        public string OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }
        public int CustomerId { get; set; }
        public int PaymentTypeId { get; set; }        
        public decimal? FinalTotal { get; set; }
        public decimal? OutstandingTotal { get; set; }
        public decimal? TotalReceived { get; set; }        
        public decimal TotalVAT { get; set; }

        public string BusinessName { get; set; }
        public string BillingAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public string FullName { get; set; }

        public IEnumerable<OrderDetail> ListOfOrderDetailViewModel { get; set; }
        public IEnumerable<Transaction> ListOfTransactionViewModel { get; set; }           
    }
}
