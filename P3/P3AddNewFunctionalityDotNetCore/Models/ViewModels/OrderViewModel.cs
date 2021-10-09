using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class OrderViewModel
    {
        [BindNever]
        public int OrderId { get; set; }

        public ICollection<CartLine> Lines { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        public DateTime Date { get; set; }
    }
}
