using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAcc.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public string OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }        
        public int CustomerId { get; set; }
        public int PaymentTypeId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FinalTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalVAT { get; set; }
    }
}
