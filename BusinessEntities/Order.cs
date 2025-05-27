using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessEntities
{
    public class Order   
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public Product Product { get; set; }
    }
}
