using MyAcc.Data;
using MyAcc.Models;
using MyAcc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.Repository
{
    public class dashboardRepository
    {
        private readonly ApplicationDbContext _db;


        public dashboardRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }


        public decimal TotalSales()
        {
            return _db.Orders.Sum(o => o.FinalTotal).Value;            
            
        }


        public List<CustomerSalesReportViewModel> TotalOutstanding()
        {
            var result = (from o in _db.Orders
                          join c in _db.Customers on o.CustomerId equals c.CustomerId
                          join t in _db.Transaction on o.OrderId equals t.OrderId
                          //	where (t.TransactionType =="Payment Received")
                          group t by new { o.OrderId, o.OrderNo, o.OrderDate, c.BusinessName, o.PaymentTypeId, o.FinalTotal, o.TotalVAT } into g
                          select new CustomerSalesReportViewModel
                          {

                              FinalTotal = g.Key.FinalTotal,
                              TotalReceived = g.Where(d => d.TransactionType == "Payment Received").Sum(d => d.Amount),
                              // OutstandingTotal = g.Key.FinalTotal - g.Where(d => d.TransactionType == "Payment Received").Sum(d => d.Amount)
                              TotalRefund = g.Where(d => d.TransactionType == "Refund").Sum(d => d.Amount),
                              OutstandingTotal = g.Key.FinalTotal - g.Where(d => d.TransactionType == "Payment Received" || d.TransactionType == "Refund").Sum(d => d.Amount)

                          }).ToList();

            return (result);

        }
    }
}
