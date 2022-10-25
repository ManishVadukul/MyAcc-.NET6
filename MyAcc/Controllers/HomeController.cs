using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MyAcc.Models;
using MyAcc.Repository;
using MyAcc.Utility;
using MyAcc.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.Controllers
{
    namespace MyAcc.Controllers
    {
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]

        public class HomeController : Controller
        {
            public readonly IWebHostEnvironment _webHostEnvirnoment;
            private readonly customerRepository _customerRepository;
            private readonly ProductRepository _productRepository;
            private readonly PaymentTypeRepository _paymentTypeRepository;
            private readonly orderRepository _orderRepository;
            private readonly reportRepository _reportRepository;
            private readonly dashboardRepository _dashboardRepository;

            public HomeController(customerRepository customerRepository, ProductRepository productRepository, PaymentTypeRepository paymentTypeRepository, orderRepository orderRepository, IWebHostEnvironment webHostEnvirnoment, reportRepository reportRepository , dashboardRepository dashboardRepository)
            {

                _customerRepository = customerRepository;
                _productRepository = productRepository;
                _paymentTypeRepository = paymentTypeRepository;
                _orderRepository = orderRepository;
                _webHostEnvirnoment = webHostEnvirnoment;
                _reportRepository = reportRepository;
                _dashboardRepository = dashboardRepository;

            }

            public IActionResult Index()
            {
                var allObj = _dashboardRepository.TotalOutstanding();
                return View(allObj);
            }


     

          

        }
    }
}

// Layout Page Nav
//< nav class= "navbar navbar-expand-sm navbar-toggleable-sm navbar-dark  bg-primary border-bottom box-shadow mb-3" >
 
//             < div class= "container" >
  
//                  < a class= "navbar-brand" asp - area = "" asp - controller = "Home" asp - action = "Index" > My.Net Core Test</a>
//                                 <button class= "navbar-toggler" type = "button" data - toggle = "collapse" data - target = ".navbar-collapse" aria - controls = "navbarSupportedContent"
//                        aria - expanded = "false" aria - label = "Toggle navigation" >
    
//                        < span class= "navbar-toggler-icon" ></ span >
     
//                     </ button >
     
//                     < div class= "navbar-collapse collapse d-sm-inline-flex flex-sm-row-reverse" >
      
//                          < partial name = "_LoginPartial" />
       
//                           < ul class= "navbar-nav flex-grow-1" >
        
//                                < li class= "nav-item" >
         
//                                     < a class= "nav-link " asp - area = "" asp - controller = "Home" asp - action = "Index" > Dashboard </ a >
                   
//                                           </ li >
//                                           @if(User.IsInRole(SD.Role_Admin))
//                        {
//                            @*< li class= "nav-item" >
 
//                                     < a class= "nav-link" asp - area = "" asp - controller = "Company" asp - action = "Index" > Companies </ a >
           
//                                           </ li > *@
//                        }
//                        @if(User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
//                        {
//                            < li class= "nav-item" >
 
//                                 < a class= "nav-link" asp - area = "" asp - controller = "Customer" asp - action = "Index" > Customers </ a >
           
//                                       </ li >
           

//                                       < li class= "nav-item dropdown" >
            
//                                            < a class= "nav-link dropdown-toggle" href = "#" id = "navbarDropdown" role = "button" data - toggle = "dropdown" aria - haspopup = "true" aria - expanded = "false" >
//                                                                Product
//                                                            </ a >
                            
//                                                            < div class= "dropdown-menu" aria - labelledby = "navbarDropdown" >
                              
//                                                                  < a class= "dropdown-item " asp - area = "" asp - controller = "Product" asp - action = "Index" > Product List </ a >
                                              
//                                                                                  < a class= "dropdown-item " asp - area = "" asp - controller = "Category" asp - action = "Index" > Product Category List</a>
//                                                                                              </div>
//                            </li>

//                            <li class= "nav-item dropdown" >
//                                < a class= "nav-link dropdown-toggle" href = "#" id = "navbarDropdown" role = "button" data - toggle = "dropdown" aria - haspopup = "true" aria - expanded = "false" >
//                                                    Sales
//                                                </ a >
                
//                                                < div class= "dropdown-menu" aria - labelledby = "navbarDropdown" >
                  
//                                                      < a class= "dropdown-item " asp - area = "" asp - controller = "Order" asp - action = "Index" > Order List </ a >
                                  
//                                                                      < a class= "dropdown-item " asp - area = "" asp - controller = "Order" asp - action = "Create" > Create New Orders</a>
//                                                                                  </div>
//                            </li>

                           
//                            <li class= "nav-item" >
//                                < a class= "nav-link" asp - area = "" asp - controller = "Reports" asp - action = "Index" > Reports </ a >
          
//                                      </ li >

//                        }
//                    </ ul >
//                </ div >
//            </ div >
//        </ nav >