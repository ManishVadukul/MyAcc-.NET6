using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MyAcc.Models;

#nullable disable

namespace MyAcc.Data
{
    public partial class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        ////    modelBuilder.Entity<Product>().HasOne<Category>(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);
        ////    modelBuilder.Entity<Product>().HasOne<TaxType>(p => p.TaxType).WithMany(t => t.Products).HasForeignKey(p => p.TaxId);
        //}

        public  DbSet<Category> Categories { get; set; }
        public  DbSet<Customer> Customers { get; set; }
        public  DbSet<Product> Products { get; set; }
        public  DbSet<Supplier> Suppliers { get; set; }
        public  DbSet<TaxType> TaxTypes { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PaymentTypeList> PaymentTypes { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Unit> Unit { get; set; }




    }
     
}
