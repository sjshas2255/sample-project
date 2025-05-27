using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessEntities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; }
    }
}
