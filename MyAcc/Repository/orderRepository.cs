using MyAcc.Data;
using MyAcc.Models;
using MyAcc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.Repository
{
    public class orderRepository
    {
        private readonly ApplicationDbContext _db;


        public orderRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }


        public  OrderViewModel GetEditOrderById(int id)
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
                              BillingAddress=c.Billingaddress,
                              DeliveryAddress=c.Deliveryaddress,
                              FullName=c.FullName,
                              PaymentTypeId = o.PaymentTypeId,
                              FinalTotal = o.FinalTotal,
                              TotalVAT = o.TotalVAT
                          }).FirstOrDefault();

            return (result);
        }


        public String InvoiceNameById(int id)
        {
            var result = _db.Orders.Where(o => o.OrderId == id).FirstOrDefault().OrderNo;

            return result;
        }
        public List<OrderViewModel> GetEditOrder()
        {
           
            var result = (from o in _db.Orders
                          join c in _db.Customers on o.CustomerId equals c.CustomerId
                         join t in _db.Transaction on o.OrderId equals t.OrderId
                         //	where (t.TransactionType =="Payment Received")
                         group t by new { o.OrderId, o.OrderNo, o.OrderDate, c.BusinessName, o.PaymentTypeId, o.FinalTotal, o.TotalVAT } into g
                         select new OrderViewModel
                         {
                             OrderId = g.Key.OrderId,
                             OrderNo = g.Key.OrderNo,
                             OrderDate = g.Key.OrderDate,
                             BusinessName = g.Key.BusinessName,
                             PaymentTypeId = g.Key.PaymentTypeId,
                             FinalTotal = g.Key.FinalTotal,
                             TotalVAT = g.Key.TotalVAT,
                             //TotalReceived = g.Where(d => d.TransactionType == "Payment Received").Sum(d => d.Amount),
                             //OutstandingTotal = g.Key.FinalTotal - g.Where(d => d.TransactionType == "Payment Received").Sum(d => d.Amount)
                             TotalReceived = g.Where(d => d.TransactionType == "Payment Received" || d.TransactionType == "Refund").Sum(d => d.Amount),
                             OutstandingTotal = g.Key.FinalTotal - g.Where(d => d.TransactionType == "Payment Received" || d.TransactionType == "Refund").Sum(d => d.Amount)
                         }).OrderByDescending(g =>g.OrderDate).ToList();

            //var result = (from o in _db.Orders
            //              join c in _db.Customers on o.CustomerId equals c.CustomerId //into temp from od in temp.DefaultIfEmpty()						                                                      
            //              join t in _db.Transaction on o.OrderId equals t.OrderId
            //              select new { o.OrderId, o.OrderNo, o.OrderDate, c.BusinessName, o.PaymentTypeId, o.FinalTotal, o.TotalVAT, t.Amount, t.TransactionType } into x
            //              group x by new { x.OrderId, x.OrderNo, x.OrderDate, x.BusinessName, x.PaymentTypeId, x.FinalTotal, x.TotalVAT, x.Amount, x.TransactionType } into g
            //              select new OrderViewModel
            //              {
            //                  OrderId = g.Key.OrderId,
            //                  OrderNo = g.Key.OrderNo,
            //                  OrderDate = g.Key.OrderDate,
            //                  BusinessName = g.Key.BusinessName,
            //                  PaymentTypeId = g.Key.PaymentTypeId,
            //                  FinalTotal = g.Key.FinalTotal,
            //                  TotalVAT = g.Key.TotalVAT,
            //                  OutstandingTotal = g.Key.FinalTotal - g.Where(d => d.OrderId == g.Key.OrderId && g.Key.TransactionType == "Payment Received").Sum(i => i.Amount)
            //              }).ToList();
            return (result);
        }


        public List<CustomerSalesReportViewModel> GetOrderOutstandingById(int id)
        {
            var result = (from o in _db.Orders
                          join c in _db.Customers on o.CustomerId equals c.CustomerId
                          join t in _db.Transaction on o.OrderId equals t.OrderId
                          where o.OrderId == id
                          group t by new { o.OrderId, o.OrderNo, o.OrderDate, c.BusinessName, o.PaymentTypeId, o.FinalTotal, o.TotalVAT } into g
                          select new CustomerSalesReportViewModel
                          {

                              FinalTotal = g.Key.FinalTotal,
                              //TotalReceived = g.Where(d => d.TransactionType == "Payment Received").Sum(d => d.Amount),
                             // Balance = g.Where(d => d.TransactionType == "Payment Received" || d.TransactionType == "Refund").Sum(d => d.Amount),
                              OutstandingTotal = g.Key.FinalTotal - g.Where(d => d.TransactionType == "Payment Received" || d.TransactionType == "Refund").Sum(d => d.Amount)

                              //TotalRefund = g.Where(d => d.TransactionType == "Refund").Sum(d => d.Amount),
                              //OutstandingTotal = g.Key.FinalTotal - g.Where(d => d.TransactionType == "Payment Received" || d.TransactionType == "Refund").Sum(d => d.Amount)

                          }).ToList();

            return (result);

        }

        public OrderOutstanding GetOutstandingById(int id)
        {
            var result = (from o in _db.Orders
                          join c in _db.Customers on o.CustomerId equals c.CustomerId
                          join t in _db.Transaction on o.OrderId equals t.OrderId
                          where o.OrderId == id
                          group t by new { o.OrderId, o.OrderNo, o.OrderDate, c.BusinessName, o.PaymentTypeId, o.FinalTotal, o.TotalVAT } into g
                          select new OrderOutstanding
                          {

                              FinalTotal = g.Key.FinalTotal,
                              //TotalReceived = g.Where(d => d.TransactionType == "Payment Received").Sum(d => d.Amount),
                              // Balance = g.Where(d => d.TransactionType == "Payment Received" || d.TransactionType == "Refund").Sum(d => d.Amount),
                              OutstandingTotal = g.Key.FinalTotal - g.Where(d => d.TransactionType == "Payment Received" || d.TransactionType == "Refund").Sum(d => d.Amount)

                              //TotalRefund = g.Where(d => d.TransactionType == "Refund").Sum(d => d.Amount),
                              //OutstandingTotal = g.Key.FinalTotal - g.Where(d => d.TransactionType == "Payment Received" || d.TransactionType == "Refund").Sum(d => d.Amount)

                          }).FirstOrDefault();

            return (result);

        }


        public List<OrderDetail> GetEditOrderDetail(int orderId)
        {
            var result = (from od in _db.OrderDetails                          
                          where (od.OrderId == orderId)
                          select new OrderDetail
                          {                            
                              
                              OrderDetailId = od.OrderDetailId,
                              ProductId = od.ProductId,
                              ProductName = od.ProductName,
                              Cost = od.Cost,
                              Price = od.Price,
                              Qty = od.Qty,
                              Tax = od.Tax,
                              VAT =od.VAT,
                              Total = od.Total,
                              Unitname = od.Unitname,
                              Weight = od.Weight

                          }).ToList();

            return (result);
        }
        public List<OrderDetail> EditOrderDetailById(int orderDetailId)
        {
            var result = (from od in _db.OrderDetails
                          where (od.OrderDetailId == orderDetailId)
                          select new OrderDetail
                          {

                              OrderId=od.OrderId,
                              OrderDetailId = od.OrderDetailId,
                              ProductId = od.ProductId,
                              ProductName = od.ProductName,
                              Cost = od.Cost,
                              Price = od.Price,
                              Qty = od.Qty,
                              Tax = od.Tax,
                              VAT = od.VAT,
                              Total = od.Total,
                              Unitname = od.Unitname,
                              Weight = od.Weight

                          }).ToList();

            return (result);
        }
        

        public string GetOrderNumberGenerator()
        {
            var _orderNo = _db.Orders.OrderByDescending(o => o.OrderId).Select(m => m.OrderId).FirstOrDefault();

            if (_orderNo == 0)
            {
                _orderNo = 1;
                //var _orderNumber = "KA-" + string.Format("{0:dd-MMMM-yyyy}", DateTime.Now) + "-" + _orderNo;
                var _orderNumber = "KA-Invoice-" + _orderNo;
                return _orderNumber;
            }
            else
            {
                //var _orderNumber = "KA-" + string.Format("{0:dd-MMMM-yyyy}", DateTime.Now) + "-" + _orderNo;
                var _orderNumber = "KA-Invoice-" + _orderNo;
                return _orderNumber;
            }            
        }

        public void DeleteOrderDetailById(int? id)
        {
            var _od = _db.OrderDetails.Where(x => x.OrderDetailId == id).FirstOrDefault();
            _db.OrderDetails.Remove(_od);
            _db.SaveChanges();
        }


        public void DeleteOrder(int? id)
        {
            var _od = _db.Orders.Where(x => x.OrderId == id).FirstOrDefault();
            _db.Orders.Remove(_od);
            _db.SaveChanges();
        }

        public bool AddOrder(OrderViewModel objOrderViewModel)
        {
            Order objOrder = new Order();
            objOrder.CustomerId = objOrderViewModel.CustomerId;
            objOrder.FinalTotal = objOrderViewModel.FinalTotal;
            objOrder.OrderDate = DateTime.Now;
            objOrder.OrderNo = objOrderViewModel.OrderNo;// string.Format("{0:ddmmmyyyyhhmmss}", DateTime.Now);
            objOrder.PaymentTypeId = objOrderViewModel.PaymentTypeId;
            objOrder.TotalVAT = objOrderViewModel.TotalVAT;
            _db.Orders.Add(objOrder);
            _db.SaveChanges();

            int _orderId = objOrder.OrderId;

            foreach (var item in objOrderViewModel.ListOfOrderDetailViewModel)
            {
                OrderDetail objOrderDetail = new OrderDetail();
                objOrderDetail.OrderId = _orderId;
                objOrderDetail.ProductId = item.ProductId;
                objOrderDetail.Cost = item.Cost;
                objOrderDetail.Price = item.Price;
                objOrderDetail.Qty = item.Qty;
                objOrderDetail.Tax = item.Tax;
                objOrderDetail.VAT = item.VAT;
                objOrderDetail.Total = item.Total;
                objOrderDetail.ProductName = item.ProductName;
                objOrderDetail.Weight = item.Weight;
                objOrderDetail.Unitname = item.Unitname;
                _db.OrderDetails.Add(objOrderDetail);
                _db.SaveChanges();
            }

            Transaction objTransaction = new Transaction();
            objTransaction.OrderId = _orderId;
            objTransaction.TransactionDate = DateTime.Now;
            objTransaction.Amount = objOrder.FinalTotal;
            objTransaction.CustomerId = objOrder.CustomerId;
            objTransaction.TransactionType = "Sales";
            objTransaction.PaymentTypeId = 0;
            _db.Transaction.Add(objTransaction);
            _db.SaveChanges();



            return true;
        }


        public bool UpdateOrder(OrderViewModel objOrderViewModel)
        {

            var objAllorder = _db.Orders.Where(x => x.OrderId == objOrderViewModel.OrderId).FirstOrDefault();
            objAllorder.FinalTotal = objOrderViewModel.FinalTotal;
            objAllorder.TotalVAT = objOrderViewModel.TotalVAT;
            objAllorder.OrderId= objOrderViewModel.OrderId;
            _db.SaveChanges();


                      

            foreach (var item in objOrderViewModel.ListOfOrderDetailViewModel)
            {
                OrderDetail objOrderDetail = new OrderDetail();
                
                if (objOrderDetail.OrderDetailId == item.OrderDetailId)
                {
                    objOrderDetail.OrderId = objOrderViewModel.OrderId; ;
                    objOrderDetail.ProductId = item.ProductId;
                    objOrderDetail.Cost = item.Cost;
                    objOrderDetail.Price = item.Price;
                    objOrderDetail.Qty = item.Qty;
                    objOrderDetail.Tax = item.Tax;
                    objOrderDetail.VAT = item.VAT;
                    objOrderDetail.Total = item.Total;
                    objOrderDetail.ProductName = item.ProductName;
                    objOrderDetail.OrderDetailId = item.OrderDetailId;
                    objOrderDetail.Weight = item.Weight;
                    objOrderDetail.Unitname = item.Unitname;
                    _db.OrderDetails.Add(objOrderDetail);
                    _db.SaveChanges();
                }
                else
                {
                    objOrderDetail.OrderId = objOrderViewModel.OrderId; ;
                    objOrderDetail.ProductId = item.ProductId;
                    objOrderDetail.Cost = item.Cost;
                    objOrderDetail.Price = item.Price;
                    objOrderDetail.Qty = item.Qty;
                    objOrderDetail.Tax = item.Tax;
                    objOrderDetail.VAT = item.VAT;
                    objOrderDetail.Total = item.Total;
                    objOrderDetail.ProductName = item.ProductName;
                    objOrderDetail.Weight = item.Weight;
                    objOrderDetail.Unitname = item.Unitname;

                    _db.SaveChanges();
                }
            }

            
            var objTransaction = _db.Transaction.Where(x => x.OrderId == objOrderViewModel.OrderId).FirstOrDefault();
            
            objTransaction.OrderId = objOrderViewModel.OrderId; 
            objTransaction.TransactionDate = DateTime.Now;
            objTransaction.Amount = objOrderViewModel.FinalTotal;            
            _db.SaveChanges();



            return true;
        }


        public bool UpdateOrderDetailsById(OrderDetail od)
        {           
            
                OrderDetail objOrderDetail = new OrderDetail();
              objOrderDetail.OrderId = od.OrderId;
                    objOrderDetail.ProductId = od.ProductId;
                    objOrderDetail.Cost = od.Cost;
                    objOrderDetail.Price = od.Price;
                    objOrderDetail.Qty = od.Qty;
                    objOrderDetail.Tax = od.Tax;
                    objOrderDetail.VAT = od.VAT;
                    objOrderDetail.Total = od.Total;
                    objOrderDetail.ProductName = od.ProductName;
                    objOrderDetail.OrderDetailId = od.OrderDetailId;
                    objOrderDetail.Weight = od.Weight;
                    objOrderDetail.Unitname = od.Unitname;
                    _db.OrderDetails.Update(objOrderDetail);
                    _db.SaveChanges();
            return true;
        }

        //public async Task<Customer> UpdateCustomer(Customer customer)
        //{
        //    var objFromDb = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);
        //    if (objFromDb != null)
        //    {
        //        objFromDb.FullName = customer.FullName;
        //        objFromDb.BusinessName = customer.BusinessName;
        //        objFromDb.Email = customer.Email;
        //        objFromDb.Phone = customer.Phone;
        //        objFromDb.Billingaddress = customer.Billingaddress;
        //        objFromDb.Deliveryaddress = customer.Deliveryaddress;
        //        objFromDb.Code = customer.Code;
        //        objFromDb.HomeNo = customer.HomeNo;
        //        objFromDb.Mobile = customer.Mobile;
        //        await _db.SaveChangesAsync();
        //        return (customer);
        //    }
        //    return (customer);
        //}



    }
}
