using MyAcc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MyAcc.ViewModels
{
    public class PaymentViewModel
    {
        public OrderViewModel _orderHeader { get; set; }
        public List<TransactionViewModel> _transaction { get; set; }
        public Transaction _tranModel { get; set; }
        public OrderOutstanding _orderOutstanding { get; set; }
    }
}
