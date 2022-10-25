//using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }                
        public DateTime TransactionDate { get; set; }
        public int OrderId { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        [Required(ErrorMessage = "This field is required")]
        public decimal? Amount { get; set; }
        public int CustomerId { get; set; }
        public string TransactionType { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int PaymentTypeId { get; set; }

        public string Notes { get; set; }

    }
}
