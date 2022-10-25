using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using MyAcc.Models;
using MyAcc.ViewModels;
using System.IO;
using System.Data;
using FastReport.Utils;
using FastReport;
using FastReport.Export.Html;
using FastReport.Export.PdfSimple;
using Microsoft.Extensions.Configuration;

namespace MyAcc.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment; // Use the web hosting environment interface to get the root path
        private readonly string _connectionString;


        public ValuesController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }
        // Create a collection of data of type Reports. Add two reports.
        Reports[] reportItems = new Reports[]
        {
             new Reports { Id = 1, ReportName = "Invoice.frx" },
             new Reports { Id = 2, ReportName = "DeliveryNote.frx" }
        };

        [HttpGet]
        public IEnumerable<Reports> Get()
        {
            return reportItems; // Returns a list of reports.
        }

        // Attribute has required id parameter
        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromQuery] ReportQuery query)
        {
            string mime = "application/" + query.Format; // MIME header with default value
                                                         // Find report
            Reports reportItem = reportItems.FirstOrDefault((p) => p.Id == id); // we get the value of the collection by id
            if (reportItem != null)
            {
                string webRootPath = _hostingEnvironment.WebRootPath; // determine the path to the wwwroot folder
                string reportPath = (webRootPath + "/Reports/" + reportItem.ReportName); // determine the path to the report
               // string dataPath = (webRootPath + "/App_Data/nwind.xml");// determine the path to the database
                using (MemoryStream stream = new MemoryStream()) // Create a stream for the report
                {
                    try
                    {
                        using (DataSet dataSet = new DataSet())
                        {
                            // Fill the source by data
                            //dataSet.ReadXml(dataPath);
                            // Turn on web mode FastReport
                            Config.WebMode = true;
                            using (Report report = new Report())
                            {
                                report.Load(reportPath); // Download the report
                                report.Report.Dictionary.Connections[0].ConnectionString = "";
                                // report.RegisterData(dataSet, "NorthWind"); // Register data in the report
                                if (query.Parameter != null)
                                {
                                    report.SetParameterValue("Parameter", query.Parameter); // Set the value of the report parameter if the parameter value is passed to the URL
                                }
                                report.SetParameterValue("orderid", id);
                                report.Prepare();//Prepare the report
                                                 // If pdf format is selected
                                if (query.Format == "pdf")
                                {
                                    // Export report to PDF
                                    PDFSimpleExport pdf = new PDFSimpleExport();
                                    // Use the stream to store the report, so as not to create unnecessary files
                                    report.Export(pdf, stream);
                                }
                                // If html report format is selected
                                else if (query.Format == "html")
                                {
                                    // Export Report to HTML
                                    HTMLExport html = new HTMLExport();
                                    html.SinglePage = true; // Single page report
                                    html.Navigator = false; // Top navigation bar
                                    html.EmbedPictures = true; // Embeds images into a document
                                    report.Export(html, stream);
                                    mime = "text/" + query.Format; // Override mime for html
                                }
                            }
                        }
                        // Get the name of the resulting report file with the necessary extension var file = String.Concat(Path.GetFileNameWithoutExtension(reportPath), ".", query.Format);
                        // If the inline parameter is true, then open the report in the browser
                        if (query.Inline)
                            return File(stream.ToArray(), mime);
                        else
                            // Otherwise download the report file 
                            return File(stream.ToArray(), mime, "test"); // attachment
                    }
                    // Handle exceptions
                    catch
                    {
                        return new NoContentResult();
                    }
                    finally
                    {
                        stream.Dispose();
                    }
                }
            }
            else
                return NotFound();
        }
    }

    public class ReportQuery
    {
        // Format of resulting report: png, pdf, html
        public string Format { get; set; }
        // Enable Inline preview in browser (generates "inline" or "attachment")
        public bool Inline { get; set; }
        // Value of "Parameter" variable in report
        public string Parameter { get; set; }
    }


 

}