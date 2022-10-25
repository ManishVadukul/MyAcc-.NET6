using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyAcc.Data;
using MyAcc.Models;
using MyAcc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.Repository
{
    public class ProductRepository
    {
        private readonly ApplicationDbContext _db;


        public ProductRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

      

        public List<ItemViewModel> GetProductDataByProductId(int productId)
        {
            var result = (from p in _db.Products
                          join t in _db.TaxTypes on p.TaxId equals t.Id
                          join u in _db.Unit on p.UnitId equals u.UnitId
                          where (p.ProductId == productId)
                          select new ItemViewModel
                          {
                              ProductId = p.ProductId,
                              Code = p.Code,
                              Title = p.Title,
                              Price = p.Price,
                              Cost = p.Cost,
                              Tax = t.Perce,
                              Weight=p.Weight,
                              UnitId=p.UnitId,
                              Unitname = u.UnitName
                              
                          }).ToList();

            return (result);
        }

        public IEnumerable<SelectListItem> GetAllItems()
        {
            IEnumerable<SelectListItem> objSelectItemList = new List<SelectListItem>();
            objSelectItemList = (from obj in _db.Products
                                 select new SelectListItem()
                                 {
                                     Text = obj.Title,
                                     Value = obj.ProductId.ToString(),
                                     Selected = true
                                 }).ToList();
            return objSelectItemList;
        }


        public async Task<List<Product>> GetProducts()
        {

            var result = await (from p in _db.Products
                          join c in _db.Categories on p.CategoryId equals c.CategoryId into Cat from c in Cat.DefaultIfEmpty()
                          join t in _db.TaxTypes on p.TaxId equals t.Id into Tax from t in Tax.DefaultIfEmpty()
                          join u in _db.Unit on p.UnitId equals u.UnitId into Unit from u in Unit.DefaultIfEmpty()
                          select new Product
                          {
                              ProductId=p.ProductId,
                              Code = p.Code,
                              Title = p.Title,
                              Price = p.Price,
                              Cost = p.Cost,
                              TaxId=p.TaxId,
                              CategoryId=p.CategoryId,
                              Perce = t.Perce,
                              Name = c.Name,
                              UnitId=p.UnitId,
                              UnitName=u.UnitName,
                              Weight=p.Weight
                          }).ToListAsync();

                       
            return (result);
        }



        public async Task<IEnumerable<Category>> DDLProductCategories()
        {
            var result = await (from c in _db.Categories
                                select c).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Unit>> DDLUnitTypes()
        {
            var result = await (from c in _db.Unit
                                select c).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<TaxType>> DDLProductTaxType()
        {
            var result = await (from c in _db.TaxTypes
                                select c).ToListAsync();

            return result;
        }


        public async Task<Product> GetProductById(int? id)
        {
            return await _db.Products.Where(c => c.ProductId == id).FirstOrDefaultAsync();
        }
        

        public async Task<ProductVM> InsertProduct(ProductVM productVM)
        {
            _db.Products.Add(productVM.Product);
            await _db.SaveChangesAsync();
            return (productVM);
        }
        public async Task<ProductVM> UpdateProduct(ProductVM productVM)
        {
            var objFromDb = await _db.Products.FirstOrDefaultAsync(c => c.ProductId == productVM.Product.ProductId);
            if (objFromDb != null)
            {
                objFromDb.Code = productVM.Product.Code;
                objFromDb.Title = productVM.Product.Title;
                objFromDb.Price = productVM.Product.Price;
                objFromDb.Cost = productVM.Product.Cost;
                objFromDb.TaxId = productVM.Product.TaxId;
                objFromDb.CategoryId = productVM.Product.CategoryId;
                objFromDb.UnitId = productVM.Product.UnitId;
                objFromDb.Weight = productVM.Product.Weight;
                await _db.SaveChangesAsync();
                return (productVM);
            }
            return (productVM);
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var objFromDb = await _db.Products.FindAsync(id);

            
                _db.Products.Remove(objFromDb);
                await _db.SaveChangesAsync();
                return (objFromDb);
            
            
        }





        //EF


    }
}
