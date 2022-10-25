using Microsoft.EntityFrameworkCore;
using MyAcc.Data;
using MyAcc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.Repository
{
    public class categoryRepository
    {
        private readonly ApplicationDbContext _dbContext;


        public categoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Category>> GetCategory()
        {
            
            return await _dbContext.Categories.ToListAsync();
        }


        public async Task<Category> GetCategoryById(int? id)
        {
            return await _dbContext.Categories.Where(c => c.CategoryId == id).FirstOrDefaultAsync();
        }
        

        public async Task<Category> InsertCategory(Category category)
        {
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
            return (category);
        }
        public async Task<Category> UpdateCategory(Category category)
        {
            var objFromDb = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);
            if (objFromDb != null)
            {
                objFromDb.Name = category.Name;                
                await _dbContext.SaveChangesAsync();
                return (category);
            }
            return (category);
        }

        public async Task<Category> DeleteCategory(int id)
        {
            var objFromDb = await _dbContext.Categories.FindAsync(id);

            
                _dbContext.Categories.Remove(objFromDb);
                await _dbContext.SaveChangesAsync();
                return (objFromDb);
            
            
        }
    }
}
