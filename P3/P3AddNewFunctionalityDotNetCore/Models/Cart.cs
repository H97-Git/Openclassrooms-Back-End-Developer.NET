using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace P3AddNewFunctionalityDotNetCore.Models
{
    public class Cart : ICart
    {
        private readonly List<CartLine> _cartLines;

        public Cart()
        {
            _cartLines = new List<CartLine>();
        }

        public void AddItem(Product product, int quantity)
        {
                if (product == null) return;
                if(product.Quantity != 0 && quantity > product.Quantity) return;
                CartLine line = _cartLines.FirstOrDefault(p => p.Product.Id == product.Id);

                if (line == null)
                {
                    _cartLines.Add(new CartLine { Product = product, Quantity = quantity });
                }
                else
                {
                    line.Quantity += quantity;
                }
        }

        public void RemoveLine(Product product) => _cartLines.RemoveAll(l => l.Product.Id == product.Id);

        public double GetTotalValue()
        {
            return _cartLines.Any() ? _cartLines.Sum(l => l.Product.Price * l.Quantity) : 0;
        }

        public double GetAverageValue()
        {
            if (!_cartLines.Any()) return 0;
            var sum = _cartLines.Sum(l => l.Quantity);
            if (sum == 0) return 0; // Throw Error ?
            return GetTotalValue() / sum;
        }

        public List<CartLine> GetCartLine()
        {
            return _cartLines;
        }

        public void Clear() => _cartLines.Clear();

        public IEnumerable<CartLine> Lines => _cartLines;
    }

    public class CartLine
    {
        public int OrderLineId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
