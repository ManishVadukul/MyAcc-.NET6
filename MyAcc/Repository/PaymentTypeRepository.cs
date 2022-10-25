using Microsoft.AspNetCore.Mvc.Rendering;
using MyAcc.Data;
using MyAcc.Models;
using MyAcc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.Repository
{
    public class PaymentTypeRepository
    {
        
        //private readonly IRepository<PaymentTypeList> _payment;
        private readonly ApplicationDbContext _db;

        public PaymentTypeRepository(ApplicationDbContext db)
        {
          //  _payment = payment;
            _db=db;
        }


        public IEnumerable<OrderViewModel> GetPaymentDetails(int id)
        {
            var result = (from o in _db.Orders
                          join c in _db.Customers on o.CustomerId equals c.CustomerId //into temp from od in temp.DefaultIfEmpty()						
                          where (o.OrderId == id)
                          select new OrderViewModel
                          {
                              OrderId = o.OrderId,
                              OrderNo = o.OrderNo,
                              OrderDate = o.OrderDate,
                              BusinessName = c.BusinessName,                              
                              FullName = c.FullName,                              
                              FinalTotal = o.FinalTotal                              
                          }).ToList();

            return (result);
        }


        public IEnumerable<SelectListItem> GetAllPaymentType()
        {
            IEnumerable<SelectListItem> objSelectItemList = new List<SelectListItem>();
            objSelectItemList = (from obj in  _db.PaymentTypes  
                                 select new SelectListItem()
                                 {
                                     Text = obj.PaymentTypeName,
                                     Value = obj.PaymentTypeId.ToString(),
                                     Selected = true
                                 }).ToList();
            return objSelectItemList;
        }

        public List<TransactionViewModel> GetTransactionByOrderId(int id)
        {
            //var result = _db.Transaction.Where(t => t.OrderId == id && t.TransactionType == "Payment Received").ToList();

            var result = (from t in _db.Transaction
                          join p in _db.PaymentTypes on t.PaymentTypeId equals p.PaymentTypeId
                          where t.OrderId == id 
                          select new TransactionViewModel
                          {
                               TransactionId = t.TransactionId,
                               TransactionDate =t.TransactionDate,
                               OrderId=t.OrderId,       
                               Amount =t.Amount,
                               CustomerId =t.CustomerId,
                               TransactionType =t.TransactionType,
                               PaymentTypeId=t.PaymentTypeId,
                               PaymentType=p.PaymentTypeName ,
                               Notes=t.Notes
                               
                               
                         }).ToList();
                
                         

            return result;
        }

        public bool AddPayment(TransactionViewModel objTransaction)
        {

            Transaction objTran = new Transaction();
            objTran.OrderId = objTransaction.OrderId;
            objTran.TransactionDate = DateTime.Now;
            objTran.Amount = objTransaction.Amount;
            objTran.CustomerId = _db.Transaction.Where(t => t.OrderId == objTransaction.OrderId).FirstOrDefault().CustomerId;
            objTran.TransactionType = "Payment Received";
            objTran.PaymentTypeId = objTransaction.PaymentTypeId;
            objTran.Notes = objTransaction.Notes;
            _db.Transaction.Add(objTran);
            _db.SaveChanges();
            return true;
        }

        public bool AddRefund(TransactionViewModel objTransaction)
        {

            Transaction objTran = new Transaction();
            objTran.OrderId = objTransaction.OrderId;
            objTran.TransactionDate = DateTime.Now;
            objTran.Amount = - + objTransaction.Amount;
            objTran.CustomerId = _db.Transaction.Where(t => t.OrderId == objTransaction.OrderId).FirstOrDefault().CustomerId;
            objTran.TransactionType = "Refund";
            objTran.PaymentTypeId = objTransaction.PaymentTypeId;
            objTran.Notes = objTransaction.Notes;
            _db.Transaction.Add(objTran);
            _db.SaveChanges();
            return true;
        }
    }
}
