using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyAcc.Models;
using MyAcc.ViewModels;
using MyAcc.Repository;
using MyAcc.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAcc.Data;
using System.IO;
using System.Collections;
using Microsoft.AspNetCore.Hosting;
using FastReport.Web;
using FastReport.Export.PdfSimple;
using Microsoft.Extensions.Configuration;

namespace MyAcc.Controllers
{
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]

    public class OrderController : Controller
    {

        public readonly IWebHostEnvironment _webHostEnvirnoment;        
        private readonly customerRepository _customerRepository;
        private readonly ProductRepository _productRepository;
        private readonly PaymentTypeRepository _paymentTypeRepository;
        private readonly orderRepository _orderRepository;
        private readonly string _connectionString;

        public OrderController(IConfiguration configuration, customerRepository customerRepository, ProductRepository productRepository, PaymentTypeRepository paymentTypeRepository, orderRepository orderRepository, IWebHostEnvironment webHostEnvirnoment)
        {
            
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _paymentTypeRepository = paymentTypeRepository;
            _orderRepository = orderRepository;
            _webHostEnvirnoment = webHostEnvirnoment;
            _connectionString= configuration.GetConnectionString("DefaultConnection");

        }

           

        public IActionResult Index()
        {

            var allObj = _orderRepository.GetEditOrder();// _categoryRepository.GetCategory();
            return View(allObj);
        }
       
        public IActionResult WebReport(int id)
        {
            var webReport = new WebReport();
            webReport.Report.Load("d:\\AllOutstandingInvoices.frx");
            //webReport.Report.SetParameterValue("startdate", 20211008);
            // webReport.Report.SetParameterValue("enddate", 20211010);
            //webReport.Report.SetParameterValue("customerid", 2);
            webReport.Report.Prepare();

            using (MemoryStream ms = new MemoryStream())
            {
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
                return File(ms.ToArray(), "application/pdf", Path.GetFileNameWithoutExtension("Master-Detail") + ".pdf");
            }

            // save file in stream
            //Stream stream = new MemoryStream();
            // webReport.Report.Export(new PDFSimpleExport(), stream);
            // stream.Position = 0;

            // return stream in browser
            // return File(stream, "application/pdf", "report.pdf");

           // return View(webReport);
        }


        [HttpGet]
        public IActionResult GetOrderNumberGenerate()
        {
            return Json(_orderRepository.GetOrderNumberGenerator());

        }


        [HttpGet]
        public IActionResult GetProductDataByProductId(int productId)
        {
            return Json(_productRepository.GetProductDataByProductId(productId));

        }

        [HttpGet]
        public IActionResult GetCustomerDataByCustomerId(int customerId)
        {
            return Json(_customerRepository.GetCustomerDataByCustomerId(customerId));

        }

        [HttpGet]
        public IActionResult GetEditOrderDetail(int id)
        {
            return Json(_orderRepository.GetEditOrderDetail(id));

        }

        [HttpGet]
        public IActionResult EditOrderDetailById(int id)
        {
            return Json(_orderRepository.EditOrderDetailById(id));

        }
        

        public IActionResult Edit(int id)
        {
            CustomerSalesReportViewModel cs = new CustomerSalesReportViewModel();
            var allobj1 = _orderRepository.GetOrderOutstandingById(id);

            foreach (var item in allobj1)
            {
                if(item.OutstandingTotal == item.FinalTotal)
                {
                    var allObj = _orderRepository.GetEditOrderById(id);// _categoryRepository.GetCategory();            
                    return View(allObj);
                    
                }               
            }
            ViewBag.Message = string.Format("You can`t edit this order. Because you have received some payment for this order !");
         //  return  RedirectToAction("Index");
            return View();
            
            
        }

        public IActionResult Create()
        {

            var objMultipleModes = new Tuple<IEnumerable<SelectListItem>, IEnumerable<SelectListItem>, IEnumerable<SelectListItem>>
               (_customerRepository.GetAllCustomers(), _productRepository.GetAllItems(), _paymentTypeRepository.GetAllPaymentType());


            return View(objMultipleModes);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetailsById([FromBody] OrderDetail objOrderDetail)
        {
            _orderRepository.UpdateOrderDetailsById(objOrderDetail);
            return Json("Item has been updated");
        }
            

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create([FromBody] OrderViewModel objOrderViewModel)
        {

            if (objOrderViewModel == null)
            {

            }
            else
            {
                if (objOrderViewModel.OrderId == 0)
                {
                    _orderRepository.AddOrder(objOrderViewModel);
                    return Json("Order has been created.");
                }
                else
                {
                    _orderRepository.UpdateOrder(objOrderViewModel);
                    return Json("Order has been updated.");
                }


            }
            return Json("Order has been created.");
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {

            if (id == null)
            {

            }
            else
            {
                _orderRepository.DeleteOrderDetailById(id);
                return Json("Order has been deleted.");
            }
            return Json("Order has been issue.");
        }


        public IActionResult GetProductList()
        {
            return Json(_productRepository.GetAllItems());
        }

        public IActionResult Privacy()
        {
            return View();
        }



        public IActionResult CreateInvoice(int id)
        {

            var allObj = _orderRepository.GetEditOrderById(id);// _categoryRepository.GetCategory();
            var allobj2 = _orderRepository.GetEditOrderDetail(id);


            InvoiceViewModel model = new InvoiceViewModel();

            model._orderHeader = allObj;
            model._orderDetails = allobj2;



          //  _orderRepository.GetEditOrderDetail(id);
            return View(model);

            //return new ViewAsPdf("CreateInvoice", allObj)
            //{
            //    PageSize = Rotativa.AspNetCore.Options.Size.A4
            //};
        }



        public IActionResult OrderPayment(int id)
        {
            return RedirectToAction("OrderPayment", "Payment", new {id});
        }

        public IActionResult DemoViewAsPDF(int id)
        {
            //var allObj = _orderRepository.GetEditOrderById(id);// _categoryRepository.GetCategory();
            //var allobj2 = _orderRepository.GetEditOrderDetail(id);


            //InvoiceViewModel model = new InvoiceViewModel();

            //model._orderHeader = allObj;
            //model._orderDetails = allobj2;

            //return new ViewAsPdf("CreateInvoice", model)
            //{
            //    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
            //    PageSize = Rotativa.AspNetCore.Options.Size.A4,
            //    PageMargins = { Left = 5, Bottom = 15, Right = 7, Top = 0 },

            //    //FileName = "ReportTest.pdf",
            //    //  CustomSwitches = "--print-media-type --header-center \"text\""               
            //    //CustomSwitches = "--footer-center \"Printed on: " + DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\""
            //    CustomSwitches = "--footer-center  \" ABC XYZ Limited, 1 High Street,London,WS5 8LN,Buckinghamshire, Company Reg: 000000, VAT No: GB12345678 \"" + "--footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\""
            //};


            string Invoice = _orderRepository.InvoiceNameById(id);
            var webReport = new WebReport();
            webReport.Report.Load("wwwroot/Reports/invoice.frx");
            // = configurationmanager;
            webReport.Report.Dictionary.Connections[0].ConnectionString = _connectionString;
            webReport.Report.SetParameterValue("orderid", id);
            webReport.Report.Prepare();




            //using (MemoryStream ms = new MemoryStream())
            //{
            //    PDFSimpleExport pdfExport = new PDFSimpleExport();
            //    pdfExport.Export(webReport.Report, ms);
            //    ms.Flush();
            //    return File(ms.ToArray(), "application/pdf", Path.GetFileNameWithoutExtension("Invoice") + ".pdf");
            //}


            // save file in stream
            Stream stream = new MemoryStream();
            PDFSimpleExport pdf = new PDFSimpleExport();
            webReport.Report.Export(pdf, stream);
            stream.Position = 0;

           

            // return stream in browser
            return File(stream, "application/pdf", Invoice + ".pdf");

        }



        public IActionResult DeliveryNote(int id)
        {
            //var allObj = _orderRepository.GetEditOrderById(id);// _categoryRepository.GetCategory();
            //var allobj2 = _orderRepository.GetEditOrderDetail(id);


            //InvoiceViewModel model = new InvoiceViewModel();

            //model._orderHeader = allObj;
            //model._orderDetails = allobj2;

            //return new ViewAsPdf("DeliveryNote", model)
            //{

            //    //FileName = "ReportTest.pdf",
            //    //  CustomSwitches = "--print-media-type --header-center \"text\""               
            //    CustomSwitches = "--footer-center \"Printed on: " + DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\""
            //};
            string DeliveryNoteOrder = _orderRepository.InvoiceNameById(id);
            var webReport = new WebReport();
            webReport.Report.Load("wwwroot/Reports/DeliveryNote.frx");
            webReport.Report.Dictionary.Connections[0].ConnectionString = _connectionString;
            webReport.Report.SetParameterValue("orderid", id);
            webReport.Report.Prepare();

            using (MemoryStream ms = new MemoryStream())
            {
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                pdfExport.Export(webReport.Report, ms);
                ms.Flush();
               
                return File(ms.ToArray(), "application/pdf", Path.GetFileNameWithoutExtension(DeliveryNoteOrder) + ".pdf");
            }
        }



        [HttpDelete]
        [ValidateAntiForgeryToken]
        public  IActionResult OrderDelete(int id)
        {
            var objFromDb = _orderRepository.GetEditOrderById(id); //EF
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
             _orderRepository.DeleteOrder(id);
            return Json(new { html = Helper.RenderRazorViewToString(this, "_viewAll",  _orderRepository.GetEditOrder()) });
        }


    }
}