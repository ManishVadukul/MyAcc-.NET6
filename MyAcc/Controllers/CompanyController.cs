using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAcc.Models;
using MyAcc.Repository;
using MyAcc.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]

    public class CompanyController : Controller
    {
       
        private readonly IRepository<Company> _Company;

        public CompanyController(IRepository<Company> Company)
        {
            _Company = Company;
        }

        public IActionResult Index()
        {
            var allObj = _Company.GetAll();// _CompanyRepository.GetCompany();
            return View(allObj);
        }

        //GET: Add or Edit
        [NoDirectAccess]
        public IActionResult Upsert(int? id)
        {
            var Company = new Company();
            if (id == null)
            {
                //this is for create
                return View(Company);
            }
            //this is for edit            
            Company = _Company.Get(id.GetValueOrDefault());


            if (Company == null)
            {
                return NotFound();
            }
            return View(Company);
        }

        //POST: Add or Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company Company)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (Company.Id == 0)
                {
                    _Company.Add(Company);

                }
                //Update
                else
                {
                    var objFromDb = _Company.GetFirstOrDefault(s => s.Id == Company.Id);


                    if (objFromDb != null)
                    {
                        objFromDb.Name = Company.Name;
                        objFromDb.StreetAddress = Company.StreetAddress;
                        objFromDb.City = Company.City;
                        objFromDb.State = Company.State;
                        objFromDb.PostalCode = Company.PostalCode;
                        objFromDb.PhoneNumber = Company.PhoneNumber;
                        objFromDb.IsAuthorizedCompany = Company.IsAuthorizedCompany;
                        _Company.Update(Company);
                    }
                }

                // return json becuase submiting form using AJAX post method
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_viewAll", _Company.GetAll()) });
            }
            //return View(Company);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Upsert", Company) });
        }


        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var objFromDb = _Company.Get(id); //EF
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _Company.Remove(id);
            return Json(new { html = Helper.RenderRazorViewToString(this, "_viewAll", _Company.GetAll()) });
        }


    }
}
