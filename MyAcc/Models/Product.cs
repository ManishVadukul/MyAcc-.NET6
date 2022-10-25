using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyAcc.Models
{
    public class Product
    {

        [Required]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Code { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }

        public int? Weight { get; set; }

        public int UnitId { get; set; }


        [Required(ErrorMessage = "This field is required")]
        public int TaxId { get; set; } //? is nullable type
                                       //
        [Required(ErrorMessage = "This field is required")]
        public int CategoryId { get; set; }


        [NotMapped]
        public string Name { get; set; } //Category Name
        [NotMapped]
        public int Perce { get; set; } //Tax
        [NotMapped]
        public SelectList CategoryList { get; set; } //Category Name
        [NotMapped]
        public SelectList TaxList { get; set; } //Tax
        [NotMapped]
        public string UnitName { get; set; } //Unit Name
        [NotMapped]
        public SelectList UnitList { get; set; } //Unit Name

    }
}
