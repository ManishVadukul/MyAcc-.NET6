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
    [Authorize(Roles = SD.Role_Admin +","+ SD.Role_Employee)]

    public class CategoryController : Controller
    {

        private readonly IRepository<Category> _category;

        public CategoryController(IRepository<Category> category)
        {
            _category = category;
        }
        public IActionResult Index()
        {
            var allObj =  _category.GetAll();// _categoryRepository.GetCategory();
            return View(allObj);
        }

        //GET: Add or Edit
        [NoDirectAccess]
        public IActionResult Upsert(int? id)
        {
            var category = new Category();
            if (id == null)
            {
                //this is for create
                return View(category);
            }
            //this is for edit            
            category =  _category.Get(id.GetValueOrDefault());


            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //POST: Add or Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (category.CategoryId == 0)
                {
                     _category.Add(category);
                    
                }
                //Update
                else
                {
                    var objFromDb = _category.GetFirstOrDefault(s => s.CategoryId == category.CategoryId );


                    if (objFromDb != null)
                    {
                        objFromDb.Name = category.Name;
                        _category.Update(category);
                    }
                }

                // return json becuase submiting form using AJAX post method
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_viewAll", _category.GetAll()) });
            }
            //return View(Category);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Upsert", category) });
        }


        [HttpDelete]
        [ValidateAntiForgeryToken]
        public  IActionResult Delete(int id)
        {
            var objFromDb = _category.Get(id); //EF
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
             _category.Remove(id);
            return Json(new { html = Helper.RenderRazorViewToString(this, "_viewAll",  _category.GetAll()) });
        }


    }
}
