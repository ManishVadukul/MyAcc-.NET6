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
using Microsoft.AspNetCore.Http;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using Microsoft.Extensions.Configuration;

namespace MyAcc.Controllers
{
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]

    public class ReportsController : Controller
    {
        public readonly IWebHostEnvironment _webHostEnvirnoment;
        private readonly customerRepository _customerRepository;
        private readonly ProductRepository _productRepository;
        private readonly PaymentTypeRepository _paymentTypeRepository;
        private readonly orderRepository _orderRepository;
        private readonly reportRepository _reportRepository;
        private readonly string _connectionString;


        

        public ReportsController(IConfiguration configuration,customerRepository customerRepository, ProductRepository productRepository, PaymentTypeRepository paymentTypeRepository, orderRepository orderRepository, IWebHostEnvironment webHostEnvirnoment , reportRepository reportRepository)
        {

            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _paymentTypeRepository = paymentTypeRepository;
            _orderRepository = orderRepository;
            _webHostEnvirnoment = webHostEnvirnoment;
            _reportRepository = reportRepository;
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        public IActionResult Index()
        {

            var customerSalesReportVM = new CustomerSalesReportViewModel()
            {
                
                CustomerList = new SelectList(_customerRepository.DDLCustomerList(), "CustomerId", "BusinessName")

            };

            return View(customerSalesReportVM);
        }
     
        
        public IActionResult _DateRange()
        {
            return View();
        }

        public IActionResult AllSalesInvoices(int id)
        {           
            return ExportToPDF("Allinvoice", "All-Invoice");
        }

        public IActionResult AllSalesInvoicesByCustomer()
        {           
            return ExportToPDF("AllInvoiceByCustomer", "AllSalesInvoiceByCustomer");
        }


        string _startdate = "Start";
        string _enddate = "End";

        [HttpPost]
        public void AllSalesInvoicesDateRange([FromBody] CustomerSalesReportViewModel consultView)
        {                       
            HttpContext.Session.SetString(_startdate, consultView.startdate);
            HttpContext.Session.SetString(_enddate, consultView.enddate);            
        }

       
        public IActionResult AllSalesInvoicesDateRangeReport()
        {            
                return ExportToPDF("AllinvoiceByDateRange", HttpContext.Session.GetString(_startdate) + " - " + HttpContext.Session.GetString(_enddate) + " - Invoices");
        }


        string _customerId = "CustomerId";
        [HttpPost]
        public void SalesInvoicesByCustomerName([FromBody] CustomerSalesReportViewModel consultView)
        {
            HttpContext.Session.SetInt32(_customerId, consultView.CustomerId);           
        }

        public IActionResult SalesInvoicesByCustomer()
        {
            int? id = HttpContext.Session.GetInt32(_customerId);
            string BusinessName = _customerRepository.CustomerNameById(id);
            return ExportToPDF("InvoiceByCustomer", BusinessName + "-Invoices");
        }

        public IActionResult AllOutstandingInvoices()
        {
            return ExportToPDF("AllOutstandingInvoices", "AllOutstandingInvoices");
        }



        [HttpPost]
        public void CustomerStatementReportByCustomer([FromBody] CustomerSalesReportViewModel consultView)
        {
            HttpContext.Session.SetInt32(_customerId, consultView.CustomerId);
        }


        public IActionResult CustomerStatementReport()
        {
            int? id = HttpContext.Session.GetInt32(_customerId);
            string BusinessName = _customerRepository.CustomerNameById(id);
            return ExportToPDF("CustomerStatementReport", BusinessName + "-Statement"); 
        }

        public IActionResult ExportToPDF(string reportName, string fileName)
        {

            var webReport = new WebReport();
            webReport.Report.Load("wwwroot/Reports/" + reportName +".frx");

            if(reportName== "AllinvoiceByDateRange")
            {
                string startdate1 = HttpContext.Session.GetString(_startdate);
                string enddate1 = HttpContext.Session.GetString(_enddate);

                DateTime startdate;
                DateTime enddate;

                startdate = DateTime.ParseExact(startdate1, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                enddate = DateTime.ParseExact(enddate1, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                webReport.Report.SetParameterValue("startdate", startdate.ToString("yyyyMMdd"));
                webReport.Report.SetParameterValue("enddate", enddate.ToString("yyyyMMdd"));
            }
            else if(reportName == "InvoiceByCustomer")
            {
                int? id = HttpContext.Session.GetInt32(_customerId);
                webReport.Report.SetParameterValue("customerid", id);
            }
            else if(reportName == "CustomerStatementReport")
            {
                int? id = HttpContext.Session.GetInt32(_customerId);
                webReport.Report.SetParameterValue("customerid", id);
            }
            else
            {

            }
            
            // = configurationmanager;
            webReport.Report.Dictionary.Connections[0].ConnectionString = _connectionString;
            webReport.Report.Prepare();
            // save file in stream
            Stream stream = new MemoryStream();
            PDFSimpleExport pdf = new PDFSimpleExport();
            webReport.Report.Export(pdf, stream);
            stream.Position = 0;

            return File(stream, "application/pdf", Path.GetFileNameWithoutExtension(fileName) + ".pdf");

        }

    }
}
