using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace The_Car_Hub.Data
{
    public class Inventory
    {
        public int Id { get; set; }
        public string VIN { get; set; }
        public int Kilometer { get; set; }

        [Column(TypeName = "Date")]
        public DateTime PurchaseDate { get; set; }

        public int PurchasePrice { get; set; }
        public string Repairs { get; set; }
        public int? RepairsCost { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? LotDate { get; set; }

        public int? SellingPrice { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? SaleDate { get; set; }

        public string Type { get; set; }
        public string Description { get; set; }
        public bool IsFeatured { get; set; }
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        public int InventoryStatusId { get; set; }
        public virtual InventoryStatus InventoryStatus { get; set; }
        public List<Media> Medias { get; set; }
    }
}