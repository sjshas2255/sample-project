using BusinessEntities;
using Common;
using Data.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    [AutoRegister]
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Product>> GetProducts(string name = null, string category = null, bool? isActive = null)
        {
            var query = _appDbContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(p => p.Category.Contains(category));

            if (isActive.HasValue)
                query = query.Where(p => p.IsActive == isActive.Value);

            return await query.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _appDbContext.Products.FindAsync(id);
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var existing = await GetProductById(product.Id);
            if (existing != null)
            {
                throw new InvalidOperationException($"A product with Id {product?.Id} already exists.");
            }
            _appDbContext.Products.Add(product);
            await _appDbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProduct(int id, Product product)
        {
            var existing = await GetProductById(id);
            
            if (existing == null)
            {
                throw new InvalidOperationException($"A product with Id {id} is not exists.");
            }

            // Update properties
            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.StockQuantity = product.StockQuantity;
            existing.Category = product.Category;
            existing.IsActive = product.IsActive;

            await _appDbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null)
            {
                throw new InvalidOperationException($"A product with Id {id} is not exists.");
            }
            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
    }
