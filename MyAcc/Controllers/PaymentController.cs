using FastReport.Export.PdfSimple;
using FastReport.Web;
using Microsoft.AspNetCore.Mvc;
using MyAcc.Models;
using MyAcc.Repository;
using MyAcc.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.Controllers
{
    public class PaymentController : Controller
    {

        private readonly customerRepository _customerRepository;
        private readonly ProductRepository _productRepository;
        private readonly PaymentTypeRepository _paymentTypeRepository;
        private readonly orderRepository _orderRepository;
        private readonly IRepository<Transaction> _transaction;

        public PaymentController(customerRepository customerRepository, ProductRepository productRepository, PaymentTypeRepository paymentTypeRepository, orderRepository orderRepository, IRepository<Transaction> transaction)
        {

            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _paymentTypeRepository = paymentTypeRepository;
            _orderRepository = orderRepository;
            _transaction = transaction;



        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult PaymentPrint(int id)
        {
            string Invoice = _orderRepository.InvoiceNameById(id);

            var webReport = new WebReport();
            webReport.Report.Load("wwwroot/Reports/PaymentReport.frx");
            webReport.Report.SetParameterValue("orderid", id);
            webReport.Report.Prepare();

            using (MemoryStream ms = new MemoryStream())
            {
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
                return File(ms.ToArray(), "application/pdf", Path.GetFileNameWithoutExtension(Invoice + "-PaymentStatement") + ".pdf");
            }
        }


        public IActionResult OrderPayment(int id)
        {

            var allObj = _orderRepository.GetEditOrderById(id);// _categoryRepository.GetCategory();                       

            var trasnactionDetails = _paymentTypeRepository.GetTransactionByOrderId(id);

            var Outstanding = _orderRepository.GetOutstandingById(id);

            PaymentViewModel model = new PaymentViewModel();

            model._orderHeader = allObj;

            model._transaction = trasnactionDetails;

            model._orderOutstanding = Outstanding;



            //  _orderRepository.GetEditOrderDetail(id);
            return View(model);

        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult OrderPayment([FromBody] TransactionViewModel objTransaction)
        {

            if (objTransaction == null)
            {

            }
            else
            {
                if (objTransaction.Amount == 0 || objTransaction.PaymentTypeId == -1)
                {
                    return Json("Please enter amount or select payment type details");
                }
                else
                {
                    _paymentTypeRepository.AddPayment(objTransaction);
                    return Json("Paymetn Added to Order.");


                    //  return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "OrderPayment", objTransaction) });

                }

            }

            return Json("issue");
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult RefundPayment([FromBody] TransactionViewModel objTransaction)
        {
            if (objTransaction == null)
            {

            }
            else
            {
                if (objTransaction.Amount == 0 || objTransaction.PaymentTypeId == -1)
                {
                    return Json("Please enter amount or select payment type details");
                }
                else
                {
                    _paymentTypeRepository.AddRefund(objTransaction);
                    return Json("Refund has been placed.");
                }
            }
            return Json("issue");
        }


        public IActionResult GetPaymentTypeList()
        {
            return Json(_paymentTypeRepository.GetAllPaymentType());
        }


      
    }
}