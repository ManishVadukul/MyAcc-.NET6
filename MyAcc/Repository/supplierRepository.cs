using Microsoft.EntityFrameworkCore;
using MyAcc.Data;
using MyAcc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAcc.Repository
{
    public class supplierRepository
    {
        private readonly ApplicationDbContext _dbContext;


        public supplierRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Supplier>> GetAllSupplier()
        {
            
            return await _dbContext.Suppliers.ToListAsync();
        }


        public async Task<Supplier> GetSupplierById(int? id)
        {
            return await _dbContext.Suppliers.Where(c => c.Id == id).FirstOrDefaultAsync();
        }
        

        public async Task<Supplier> InsertSupplier(Supplier supplier)
        {
            _dbContext.Suppliers.Add(supplier);
            await _dbContext.SaveChangesAsync();
            return (supplier);
        }
        public async Task<Supplier> UpdateSupplier(Supplier supplier)
        {
            var objFromDb = await _dbContext.Suppliers.FirstOrDefaultAsync(c => c.Id == supplier.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = supplier.Name;
                objFromDb.Email = supplier.Email;
                objFromDb.Phone = supplier.Phone;
                objFromDb.Address1 = supplier.Address1;
                objFromDb.Address2 = supplier.Address2;
                objFromDb.Town = supplier.Town;
                objFromDb.State = supplier.State;
                objFromDb.Postcode = supplier.Postcode;
                await _dbContext.SaveChangesAsync();
                return (supplier);
            }
            return (supplier);
        }

        public async Task<Supplier> DeleteSupplier(int id)
        {
            var objFromDb = await _dbContext.Suppliers.FindAsync(id);

            
                _dbContext.Suppliers.Remove(objFromDb);
                await _dbContext.SaveChangesAsync();
                return (objFromDb);
            
            
        }
    }
}
