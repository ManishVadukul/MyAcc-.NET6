using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAcc.Models;
using MyAcc.Repository;
using MyAcc.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MyAcc.Utility;

namespace MyAcc.Controllers
{
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class ProductController : Controller
    {
        private readonly ProductRepository _productRepository;
       
        public ProductController(ProductRepository productRepository)
        {
            _productRepository = productRepository;

        }
        public async Task<IActionResult> Index()
        {
            var allObj = await _productRepository.GetProducts();
            return View(allObj);
        }

    
        //GET: Add or Edit
        [NoDirectAccess]
        public async Task<IActionResult> Upsert(int? id)
        {
            var productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = new SelectList(await _productRepository.DDLProductCategories(), "CategoryId", "Name"),
                TaxTypeList = new SelectList(await _productRepository.DDLProductTaxType(), "Id", "Perce"),
                UnitTypeList = new SelectList(await _productRepository.DDLUnitTypes(), "UnitId", "UnitName")
                
            };
            

            if (id == null)
            {
                //this is for create
                return View(productVM);
            }
            //this is for edit            
            productVM.Product = await _productRepository.GetProductById(id.GetValueOrDefault());

            if (productVM.Product == null)
            {
                return NotFound();
            }
            return View(productVM);
        }

        //POST: Add or Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductVM productVM)
        {

            //Set Dropdownlist before & after update 
            productVM.CategoryList = new SelectList(await _productRepository.DDLProductCategories(), "CategoryId", "Name");
            productVM.TaxTypeList = new SelectList(await _productRepository.DDLProductTaxType(), "Id", "Perce");
            productVM.UnitTypeList = new SelectList(await _productRepository.DDLUnitTypes(), "UnitId", "UnitName");



            if (ModelState.IsValid)
            {
                //Insert
                if (productVM.Product.ProductId == 0)
                {
                    await _productRepository.InsertProduct(productVM);
                }
                //Update
                else
                {
                    await _productRepository.UpdateProduct(productVM);

                }

                // return json becuase submiting form using AJAX post method
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_viewAll", await _productRepository.GetProducts()) });
            }
            //return View(Product);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Upsert", productVM) });
        }


        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = _productRepository.GetProductById(id); //EF
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            await _productRepository.DeleteProduct(id);
            return Json(new { html = Helper.RenderRazorViewToString(this, "_viewAll", await _productRepository.GetProducts()) });
        }


    }
}