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
    public class customerRepository
    {
        private readonly ApplicationDbContext _db;


        public customerRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }


        public String CustomerNameById(int? id)
        {
            var result = _db.Customers.Where(o => o.CustomerId == id).FirstOrDefault().BusinessName;

            return result;
        }

        public string GetCustomerAccountNumberGenerator()
        {
            var _customerid = _db.Customers.OrderByDescending(o => o.CustomerId).Select(m => m.CustomerId).FirstOrDefault();

            if (_customerid == 0)
            {
                _customerid = 1;
                //var _orderNumber = "KA-" + string.Format("{0:dd-MMMM-yyyy}", DateTime.Now) + "-" + _orderNo;
                var _orderNumber = "KACC"  + _customerid;
                return _orderNumber;
            }
            else  
            {
                //var _orderNumber = "KA-" + string.Format("{0:dd-MMMM-yyyy}", DateTime.Now) + "-" + _orderNo;
                _customerid += 1;
                var _orderNumber = "KACC" + _customerid;
                return _orderNumber;
            }
        }

        public List<CustomerViewModel> GetCustomerDataByCustomerId(int customerId)
        {
            var result = (from c in _db.Customers                          
                          where (c.CustomerId == customerId)
                          select new CustomerViewModel
                          {
                              CustomerId = c.CustomerId,                              
                              Billingaddress = c.Billingaddress,
                              Deliveryaddress = c.Deliveryaddress,
                              Fullname = c.FullName
                          }).ToList();

            return (result);
        }

        public IEnumerable<SelectListItem> GetAllCustomers()
        {
            IEnumerable<SelectListItem> objSelectItemList = new List<SelectListItem>();
            objSelectItemList = (from obj in _db.Customers
                                 select new SelectListItem()
                                 {
                                     Text = obj.BusinessName,
                                     Value = obj.CustomerId.ToString(),
                                     Selected = true
                                 }).ToList();
            return objSelectItemList;
        }


        public async Task<List<Customer>> GetAllCustomer()
        {
            
            return await _db.Customers.ToListAsync();
        }


        public async Task<Customer> GetCustomerById(int? id)
        {
            return await _db.Customers.Where(c => c.CustomerId == id).FirstOrDefaultAsync();
        }



        public List<Customer> DDLCustomerList()
        {
            var result =  (from c in _db.Customers
                                select c).ToList();
            return result;
        }


        public async Task<Customer> InsertCustomer(Customer customer)
        {
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
            return (customer);
        }
        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            var objFromDb = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);
            if (objFromDb != null)
            {
                objFromDb.FullName = customer.FullName;
                objFromDb.BusinessName = customer.BusinessName;
                objFromDb.Email = customer.Email;
                objFromDb.Phone = customer.Phone;
                objFromDb.Billingaddress = customer.Billingaddress;
                objFromDb.Deliveryaddress = customer.Deliveryaddress;
                objFromDb.Code = customer.Code;
                objFromDb.HomeNo = customer.HomeNo;
                objFromDb.Mobile = customer.Mobile;
                await _db.SaveChangesAsync();
                return (customer);
            }
            return (customer);
        }

        public async Task<Customer> DeleteCustomer(int id)
        {
            var objFromDb = await _db.Customers.FindAsync(id);

            
                _db.Customers.Remove(objFromDb);
                await _db.SaveChangesAsync();
                return (objFromDb);
            
            
        }
    }
}
