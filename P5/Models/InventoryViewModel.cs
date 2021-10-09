using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace The_Car_Hub.Models
{
    public class InventoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [Remote(action: "VerifyVIN",controller: "Dashboard")]
        public string VIN { get; set; }

        [Required]
        [DisplayName("Mileage")]
        public int Kilometer { get; set; }

        [Required]
        [DisplayName("Purchase date")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [DisplayName("Purchase price")]
        [Range(1000,100000)]
        public int PurchasePrice { get; set; }

        [StringLength(500)]
        public string Repairs { get; set; }

        [DisplayName("Repairs cost")]
        public int? RepairsCost { get; set; }

        [DisplayName("Lot date")]
        [DataType(DataType.Date)]
        public DateTime? LotDate { get; set; }

        [DisplayName("Selling price")]
        public int SellingPrice { get; set; }

        [DisplayName("Sale date")]
        [DataType(DataType.Date)]
        public DateTime? SaleDate { get; set; }

        [Required]
        [DisplayName("Type")]
        public string Type { get; set; }

        [Required]
        [StringLength(10)]
        public string Make { get; set; }

        [Required]
        [StringLength(10)]
        public string Model { get; set; }

        [Required]
        [StringLength(10)]
        public string Trim { get; set; }

        [Range(1990,2020,ErrorMessage = "Year out of range. 1990 / 2020")]
        public int Year { get; set; }

        [Required]
        [StringLength(8)]
        public string Color { get; set; }

        [Required]
        public string Fuel { get; set; }

        [Required]
        [DisplayName("Numbers of seats")]
        [Range(1,10)]
        public int Seats { get; set; }

        [Required]
        public string Gearbox { get; set; }

        [Required(ErrorMessage = "The Engine size field is required.")]
        [DisplayName("Engine size")]
        [Range(1000,10000)]
        public int EngineSize { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [DisplayName("Featured")]
        public bool IsFeatured { get; set; }

        public List<IFormFile> Photos { get; set; }
        public List<string> PhotosName { get; set; }
    }
}