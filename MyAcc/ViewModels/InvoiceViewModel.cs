using MyAcc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.ViewModels
{
    public class InvoiceViewModel
    {
        public OrderViewModel _orderHeader { get; set; }
        public List<OrderDetail> _orderDetails { get; set; }
    }
}
