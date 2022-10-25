using MyAcc.Data;
using MyAcc.Models;
using MyAcc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.Repository
{
    public class reportRepository
    {
        private readonly ApplicationDbContext _db;


        public reportRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }


        public List<CustomerSalesReportViewModel> CustomerSalesReport()
        {
            var result = (from o in _db.Orders
                          join c in _db.Customers on o.CustomerId equals c.CustomerId 
                          select new CustomerSalesReportViewModel
                          {               
                              OrderId=o.OrderId,
                              OrderNo = o.OrderNo,
                              OrderDate = o.OrderDate,
                              FullName = c.FullName,
                              BusinessName = c.BusinessName,
                              Code = c.Code,                              
                              FinalTotal = o.FinalTotal,
                              TotalVAT = o.TotalVAT
                          }).OrderByDescending(o=>o.OrderDate).ToList();

            return (result);
        }


        public List<CustomerSalesReportViewModel> CustomerSalesReport(int? id)
        {
            var result = (from o in _db.Orders
                          join c in _db.Customers on o.CustomerId equals c.CustomerId
                          where o.CustomerId == id
                          select new CustomerSalesReportViewModel
                          {
                              OrderId = o.OrderId,
                              OrderNo = o.OrderNo,
                              OrderDate = o.OrderDate,
                              FullName = c.FullName,
                              BusinessName = c.BusinessName,
                              Code = c.Code,
                              FinalTotal = o.FinalTotal,
                              TotalVAT = o.TotalVAT
                          }).OrderByDescending(o => o.OrderDate).ToList();

            return (result);
        }



        public List<CustomerSalesReportViewModel> CustomerSalesReport(DateTime startdate, DateTime enddate)
        {
            var result = (from o in _db.Orders
                          join c in _db.Customers on o.CustomerId equals c.CustomerId
                          where o.OrderDate >= startdate &&  o.OrderDate <=  enddate
                          select new CustomerSalesReportViewModel
                          {
                              OrderId = o.OrderId,
                              OrderNo = o.OrderNo,
                              OrderDate = o.OrderDate,
                              FullName = c.FullName,
                              BusinessName = c.BusinessName,
                              Code = c.Code,
                              FinalTotal = o.FinalTotal,
                              TotalVAT = o.TotalVAT
                          }).OrderByDescending(o => o.OrderDate).ToList();

            return (result);
        }




        public List<CustomerSalesReportViewModel> AllOutstandingInvoices()
        {         
           var result = (from o in _db.Orders
                          join c in _db.Customers on o.CustomerId equals c.CustomerId
                          join t in _db.Transaction on o.OrderId equals t.OrderId
                          //	where (t.TransactionType =="Payment Received")
                          group t by new { o.OrderId, o.OrderNo, o.OrderDate, c.BusinessName, o.PaymentTypeId, o.FinalTotal, o.TotalVAT } into g
                          select new CustomerSalesReportViewModel
                          {
                              OrderId = g.Key.OrderId,
                              OrderNo = g.Key.OrderNo,
                              OrderDate = g.Key.OrderDate,
                              BusinessName = g.Key.BusinessName,
                             // PaymentTypeId = g.Key.PaymentTypeId,
                              FinalTotal = g.Key.FinalTotal,
                              TotalVAT = g.Key.TotalVAT,
                              TotalReceived = g.Where(d => d.TransactionType == "Payment Received").Sum(d => d.Amount),
                              OutstandingTotal = g.Key.FinalTotal - g.Where(d => d.TransactionType == "Payment Received").Sum(d => d.Amount)
                          }).Where(g => g.OutstandingTotal != 0).OrderByDescending(g => g.OrderDate).ToList();
            return (result);
        }


        public List<CustomerSalesReportViewModel> CustomerStatementReport(int? id)
        {
            var result = (from o in _db.Orders
                          join c in _db.Customers on o.CustomerId equals c.CustomerId
                          join t in _db.Transaction on o.OrderId equals t.OrderId
                          //	where (t.TransactionType =="Payment Received")
                          group t by new { o.OrderId, o.OrderNo, o.OrderDate, c.BusinessName, o.PaymentTypeId, o.FinalTotal, o.TotalVAT , c.CustomerId } into g
                          select new CustomerSalesReportViewModel
                          {
                              OrderId = g.Key.OrderId,
                              OrderNo = g.Key.OrderNo,
                              OrderDate = g.Key.OrderDate,
                              BusinessName = g.Key.BusinessName,
                              CustomerId = g.Key.CustomerId,
                              FinalTotal = g.Key.FinalTotal,
                              TotalVAT = g.Key.TotalVAT,
                              TotalReceived = g.Where(d => d.TransactionType == "Payment Received").Sum(d => d.Amount),
                              OutstandingTotal = g.Key.FinalTotal - g.Where(d => d.TransactionType == "Payment Received").Sum(d => d.Amount)
                          }).Where(g => g.CustomerId == id).OrderByDescending(g => g.OrderDate).ToList();
            return (result);
        }


    }
}
