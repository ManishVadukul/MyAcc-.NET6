using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using MyAcc.Models;

namespace MyAcc.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public Category Category { get; set; }
        public TaxType TaxType { get; set; }
        public SelectList  CategoryList { get; set; } //For Dropdown SelectLitemList
        public SelectList TaxTypeList { get; set; } //For Dropdown SelectLitemList
        public SelectList UnitTypeList { get; set; } //For Dropdown SelectLitemList
    }
}
