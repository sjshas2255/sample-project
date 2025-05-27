using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Products
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts(string name = null, string category = null, bool? isActive = null);
        Task<Product> GetProductById(int id);
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(int id, Product product);
        Task<bool> DeleteProduct(int id);
    }
}
