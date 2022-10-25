using Microsoft.AspNetCore.Mvc;
//using MyAcc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAcc.Models;
using MyAcc.Repository;
using Microsoft.AspNetCore.Authorization;
using MyAcc.Utility;

namespace MyAcc.Controllers
{
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class CustomerController : Controller
    {
        private readonly customerRepository _customerRepository;

        public CustomerController(customerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }


        public async Task<IActionResult> Index()
        {            
            var allObj = await _customerRepository.GetAllCustomer();
            return View(allObj);
        }

        //GET: Add or Edit
        [NoDirectAccess]
        public async Task<IActionResult> Upsert(int? id)
        {
            var customer = new Customer();
            if (id == null)
            {
                //this is for create
                customer.Code = _customerRepository.GetCustomerAccountNumberGenerator();
                return View(customer);
            }
            //this is for edit            
            customer = await _customerRepository.GetCustomerById(id.GetValueOrDefault());


            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        //POST: Add or Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Customer customer)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (customer.CustomerId == 0)
                {
                    await _customerRepository.InsertCustomer(customer);
                }
                //Update
                else
                {
                    await _customerRepository.UpdateCustomer(customer);

                }

                // return json becuase submiting form using AJAX post method
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_viewAll", await _customerRepository.GetAllCustomer()) });
            }
            //return View(Customer);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Upsert", customer) });
        }


        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = _customerRepository.GetCustomerById(id); //EF
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            await _customerRepository.DeleteCustomer(id);            
            return Json(new { html = Helper.RenderRazorViewToString(this, "_viewAll", await _customerRepository.GetAllCustomer()) });
        }


        [HttpGet]
        public IActionResult GetCustomerAccountNumberGenerate()
        {
            return Json(_customerRepository.GetCustomerAccountNumberGenerator());

        }

    }
}