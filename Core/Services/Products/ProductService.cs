using BusinessEntities;
using Common;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Products
{
    [AutoRegister]
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<IEnumerable<Product>> GetProducts(string name = null, string category = null, bool? isActive = null) => _productRepository.GetProducts(name, category, isActive);

        public Task<Product> GetProductById(int id) => _productRepository.GetProductById(id);

        public Task<Product> CreateProduct(Product product) => _productRepository.CreateProduct(product);

        public Task<Product> UpdateProduct(int id, Product product) => _productRepository.UpdateProduct(id, product);

        public Task<bool> DeleteProduct(int id) => _productRepository.DeleteProduct(id);
    }
}
