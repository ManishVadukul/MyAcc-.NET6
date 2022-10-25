using System.ComponentModel.DataAnnotations;

namespace MyAcc.Models
{
    public class PaymentTypeList
    {
        [Key]
        public int PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
    }

}
