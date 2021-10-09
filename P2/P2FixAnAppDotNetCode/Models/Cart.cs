using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace P2FixAnAppDotNetCode.Models
{
    /// <summary>
    /// The Cart class
    /// </summary>
    public class Cart : ICart
    {
        // Log Manager to help me during debugging.
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Private backing field for setting and retrieving the property value.
        private List<CartLine> _lines;
        
        public IEnumerable<CartLine> Lines => _lines;

        public Cart()
        {
            _lines = new List<CartLine>();
        }

        /// <summary>
        /// Adds a product in the cart or increment its quantity in the cart if already added
        /// </summary>
        public void AddItem(Product product, int quantity)
        {
            if (_lines != null)
            {
                if (_lines.Any(p => p.Product.Name == product.Name))
                {
                    _lines.First(p => p.Product.Name == product.Name).Quantity += quantity;
                }
                else
                {
                    _lines.Add(new CartLine
                    {
                        Product = product,
                        Quantity = quantity
                    });
                }
            }
        }

        /// <summary>
        /// Removes a product form the cart
        /// </summary>
        public void RemoveLine(Product product)
        {
            _lines?.RemoveAll(p => p.Product.Name == product.Name);
        }


        /// <summary>
        /// Get total value of a cart
        /// </summary>
        public double GetTotalValue()
        {
            // Creates a double to store the total value.
            double totalValue = 0.0;
            if (_lines != null)
            {
                foreach (var line in _lines)
                {
                    // For each lines in the cart increment the total value by the product price time the product quantity.
                    totalValue += line.Product.Price * line.Quantity;
                }
            }
            return totalValue;
        }

        /// <summary>
        /// Get average value of a cart
        /// </summary>
        public double GetAverageValue()
        {
            // Creates a int to store the total quantity.
            int totalQuantity = 0;
            if ( _lines.Count != 0 && _lines != null )
            {
                foreach (var line in _lines)
                {
                    // For each lines in the cart increment the total quantity by the product quantity.
                    totalQuantity += line.Quantity;
                }
                // Return the average value by dividing the total value by the total quantity.
                return GetTotalValue() / totalQuantity;
            }
            return 0.0;
        }

        /// <summary>
        /// Looks after a given product in the cart and returns if it finds it
        /// </summary>
        public Product FindProductInCartLines(int productId)
        {
            // Create a new List<T> based on the predicated param productId.
            List<CartLine> result = Lines.Where(p => p.Product.Id == productId).ToList();
            // Create a var wich contains the first object.
            var resultvar = result.FirstOrDefault();
            // Check if the var
            if (resultvar != null)
            {
                return resultvar.Product;
            }
            return null;
        }

        /// <summary>
        /// Get a specifid cartline by its index
        /// </summary>
        public CartLine GetCartLineByIndex(int index)
        {
            return Lines?.ToArray()[index];
        }

        /// <summary>
        /// Clears a the cart of all added products
        /// </summary>
        public void Clear()
        {
           _lines?.Clear();
        }
    }

    public class CartLine
    {
        public int OrderLineId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
