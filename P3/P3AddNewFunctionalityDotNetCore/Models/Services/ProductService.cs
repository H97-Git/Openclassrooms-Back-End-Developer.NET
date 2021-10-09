using Microsoft.Extensions.Localization;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace P3AddNewFunctionalityDotNetCore.Models.Services
{
    public class ProductService : IProductService
    {
        private readonly ICart _cart;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IStringLocalizer<ProductService> _localizer;

        public ProductService(ICart cart, IProductRepository productRepository,
            IOrderRepository orderRepository, IStringLocalizer<ProductService> localizer)
        {
            _cart = cart;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _localizer = localizer;
        }
        public List<ProductViewModel> GetAllProductsViewModel()
        {
             
            IEnumerable<Product> productEntities = GetAllProducts();
            return MapToViewModel(productEntities);
        }

        private static List<ProductViewModel> MapToViewModel(IEnumerable<Product> productEntities)
        {
            List <ProductViewModel> products = new List<ProductViewModel>();
            foreach (Product product in productEntities)
            {
                products.Add(new ProductViewModel
                {
                    Id = product.Id,
                    Stock = product.Quantity.ToString(),
                    Price = product.Price.ToString(CultureInfo.InvariantCulture),
                    Name = product.Name,
                    Description = product.Description,
                    Details = product.Details
                });
            }
            return products;
        }

        public List<Product> GetAllProducts()
        {
            IEnumerable<Product> productEntities = _productRepository.GetAllProducts();
            return productEntities?.ToList();
        }

        public ProductViewModel GetProductByIdViewModel(int id)
        {
            List<ProductViewModel> products = GetAllProductsViewModel().ToList();
            return products.Find(p => p.Id == id);
        }

        public Product GetProductById(int id)
        {
            List<Product> products = GetAllProducts().ToList();
            return products.Find(p => p.Id == id);
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _productRepository.GetProduct(id);
            return product;
        }

        public async Task<IList<Product>> GetProduct()
        {
            var products = await _productRepository.GetProduct();
            return products;
        }


        public void UpdateProductQuantities()
        {
            Cart cart = (Cart) _cart;
            foreach (CartLine line in cart.Lines)
            {
                _productRepository.UpdateProductStocks(line.Product.Id, line.Quantity);
            }
        }

        private bool NullOrEmpty()
        {
            return false;
        }
        
        public List<string> CheckProductModelErrors(ProductViewModel product)
        {
            List<string> modelErrors = new List<string>();

            /////////////////// Name////////////////////////////////
            if (product.Name == null || string.IsNullOrWhiteSpace(product.Name))
            {
                modelErrors.Add(_localizer["MissingName"]);
            }
            else if(product.Name.Length < 3 || product.Name.Length > 100)
            {
                modelErrors.Add(_localizer["NameNotInRange"]);
            }
            else
            {
                product.Name = product.Name.Trim(' ');
            }
            /////////////////// Description ////////////////////////////////
            if (product.Description == null || string.IsNullOrWhiteSpace(product.Description))
            {
                modelErrors.Add(_localizer["MissingDescription"]);
            }
            else if (product.Description.Length < 3 || product.Description.Length > 500)
            {
                modelErrors.Add(_localizer["DescriptionNotInRange"]);
            }
            else
            {
                product.Description = product.Description.Trim(' ');
            }
            
            ///////////////////     Stock     ////////////////////////////////
            if (product.Stock == null || string.IsNullOrWhiteSpace(product.Stock))
            {
                modelErrors.Add(_localizer["MissingQuantity"]);
            }
            else
            {
                product.Stock = product.Stock.Trim(' ');
                if (!int.TryParse(product.Stock, out int qt))
                {
                    modelErrors.Add(_localizer["StockNotAnInteger"]);
                }
                else
                {
                    if (qt <= 0)
                        modelErrors.Add(_localizer["StockNotGreaterThanZero"]);
                    else if (qt > 999)
                        modelErrors.Add(_localizer["StockGreaterThanMaxStock"]);
                }
            }
            ///////////////////      Price     ////////////////////////////////
            if (product.Price == null || string.IsNullOrWhiteSpace(product.Price))
            {
                modelErrors.Add(_localizer["MissingPrice"]);
            }
            else
            {
                product.Price = product.Price.Trim(' ');
                if (!Double.TryParse(product.Price, out double pc))
                {
                    modelErrors.Add(_localizer["PriceNotANumber"]);
                }
                else
                {
                    if (pc <= 0)
                        modelErrors.Add(_localizer["PriceNotGreaterThanZero"]);
                    else if (pc > 999)
                        modelErrors.Add(_localizer["PriceGreaterThanMaxPrice"]);
                }
            }
            ///////////////////      Details     ////////////////////////////////
            if (product.Details != null)
            {
                product.Details = product.Details.Trim(' ');
            }

            //Escaped char ?
            return modelErrors;
        }

        public void SaveProduct(ProductViewModel product)
        {
            if (product != null)
            {
                var productToAdd = MapToProductEntity(product);
                _productRepository.SaveProduct(productToAdd);
            }
        }

        //Add the id ?
        private static Product MapToProductEntity(ProductViewModel product)
        {
            Product productEntity = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = double.Parse(product.Price),
                Quantity = Int32.Parse(product.Stock),
                Description = product.Description,
                Details = product.Details
            };
            return productEntity;
        }

        public void DeleteProduct(int id)
        {
            var product = GetProductById(id);
            if (product != null)
            {
                if (_cart != null && _cart.GetCartLine().Count != 0)
                {
                    _cart.RemoveLine(product);
                }
                _productRepository.DeleteProduct(id);
            }
        }
    }
}
