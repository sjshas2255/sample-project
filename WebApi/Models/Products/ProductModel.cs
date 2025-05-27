using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Products
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; }

        public ProductModel()
        {

        }

        public ProductModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            StockQuantity = product.StockQuantity;
            Category = product.Category;
            IsActive = product.IsActive;
        }
        public Product ToEntity()
        {
            return new Product
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Price = this.Price,
                StockQuantity = this.StockQuantity,
                Category = this.Category,
                IsActive = this.IsActive
            };
        }
    }


    }