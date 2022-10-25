using Microsoft.AspNetCore.Mvc;
using MyAcc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAcc.Repository;
using Microsoft.AspNetCore.Authorization;
using MyAcc.Utility;

namespace MyAcc.Controllers
{
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class SupplierController : Controller
    {
        
        private readonly supplierRepository _supplierRepository;
        
        public SupplierController(supplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        public async Task<IActionResult> Index()
        {
            var allObj = await _supplierRepository.GetAllSupplier();
            return View(allObj);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            var Supplier = new Supplier();
            if (id == null)
            {
                //this is for create
                return View(Supplier);
            }
            //this is for edit
            //Supplier = _unitOfWork.Supplier.Get(id.GetValueOrDefault()); //EF
            Supplier = await _supplierRepository.GetSupplierById(id.GetValueOrDefault());


            if (Supplier == null)
            {
                return NotFound();
            }
            return View(Supplier);
        }

        //POST: Add or Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (supplier.Id == 0)
                {
                    await _supplierRepository.InsertSupplier(supplier);
                }
                //Update
                else
                {
                    await _supplierRepository.UpdateSupplier(supplier);

                }

                // return json becuase submiting form using AJAX post method
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_viewAll", await _supplierRepository.GetAllSupplier()) });
            }
            //return View(Supplier);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Upsert", supplier) });
        }


        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = _supplierRepository.GetSupplierById(id); //EF
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            await _supplierRepository.DeleteSupplier(id);
            return Json(new { html = Helper.RenderRazorViewToString(this, "_viewAll", await _supplierRepository.GetAllSupplier()) });
        }


    }
}