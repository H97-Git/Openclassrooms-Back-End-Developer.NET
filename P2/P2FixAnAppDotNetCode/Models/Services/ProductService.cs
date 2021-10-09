using P2FixAnAppDotNetCode.Models.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace P2FixAnAppDotNetCode.Models.Services
{
    /// <summary>
    /// This class provides services to manages the products
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public ProductService(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Get all product from the inventory
        /// </summary>
 
        // Change return type from array to List<T> : IProductService, IProductRepository, ProductRepository/GetAllProducts().
        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        /// <summary>
        /// Get a product form the inventory by its id
        /// </summary>
        public Product GetProductById(int id)
        {
            // Get all products in variable.
            List<Product> allProducts = _productRepository.GetAllProducts();
            // Create a product based on predicate id.
            Product product = allProducts.FirstOrDefault(p => p.Id == id);
            return product;
        }

        /// <summary>
        /// Update the quantities left for each product in the inventory depending of ordered the quantities
        /// </summary>
        public void UpdateProductQuantities(Cart cart)
        {
            // update product inventory by using _productRepository.UpdateProductStocks() method.

            // Check if the cart contains any lines.
            if (cart.Lines != null)
                foreach (var cartLine in cart.Lines)
                {
                    // Call UpdateProductStocks() for each line in the cart.
                    _productRepository.UpdateProductStocks(cartLine.Product.Id, cartLine.Quantity);
                }
        }
    }
}
