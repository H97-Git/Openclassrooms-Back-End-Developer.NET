﻿using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P3AddNewFunctionalityDotNetCore.Models.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private static P3Referential _context;

        public ProductRepository(P3Referential context)
        {
                _context = context;
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == id);
            return product;
        }

        /// <summary>
        /// Get all products from the inventory async
        /// </summary>
        public async Task<IList<Product>> GetProduct()
        {
            var products = await _context.Product.ToListAsync();
            return products;
        }
        
        /// <summary>
        /// Get all products from the inventory
        /// </summary>
        public IEnumerable<Product> GetAllProducts()
        {
            IEnumerable<Product> productEntities= _context.Product.Where(p => p.Id > 0);
            return productEntities.ToList();
        }

        /// <summary>
        /// Update the stock of a product by its id
        /// </summary>
        public void UpdateProductStocks(int id, int quantityToRemove)
        {

            if (id == 0 || quantityToRemove < 0) return;
            Product product = _context.Product.FirstOrDefault(p => p.Id == id);
            if (product == null) return;
            product.Quantity -= quantityToRemove;
            // Change product.Quantity == 0 to product.Quantity <= 0
            if (product.Quantity <= 0)
            {
                _context.Product.Remove(product);
                _context.SaveChanges();
            }
            else
            {
                _context.Product.Update(product);
                _context.SaveChanges();
            }   
        }

        public void SaveProduct(Product product)
        {
            if (product != null)
            {
                _context.Product.Add(product);
                _context.SaveChanges();
            }
        }

        public void DeleteProduct(int id)
        {
            // Change First to FirstOrDefault to avoid System.InvalidOperationException
            Product product = _context.Product.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Product.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
