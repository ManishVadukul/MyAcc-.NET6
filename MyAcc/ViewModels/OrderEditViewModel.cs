using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.ViewModels
{
    public class OrderEditViewModel
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }

        
        public DateTime? OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string BusinessName { get; set; }
        public string BillingAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public string FullName { get; set; }
        public int PaymentTypeId { get; set; }
        public decimal? FinalTotal { get; set; }
        public int OrderDetailId { get; set; }        
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? Cost { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public int Tax { get; set; }
        public decimal? VAT { get; set; }
        public decimal? Total { get; set; }
        public decimal TotalVAT { get; set; }
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }        
        public decimal? Amount { get; set; }        
        public string TransactionType { get; set; }
    }
}
