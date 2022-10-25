using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAcc.ViewModels
{
    public class TransactionViewModel
    {
        [Key]
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int OrderId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        public int CustomerId { get; set; }
        public string TransactionType { get; set; }

        public int PaymentTypeId { get; set; }

        public string PaymentType { get; set; }


        public string Notes { get; set; }
    }
}
