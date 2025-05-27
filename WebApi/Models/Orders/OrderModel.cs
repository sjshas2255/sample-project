using BusinessEntities;
using System;
using WebApi.Models.Products;

public class OrderModel
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public string Status { get; set; }
    public ProductModel Product { get; set; }

    public OrderModel() { }

    public OrderModel(Order order)
    {
        OrderId = order.OrderId;
        OrderDate = order.OrderDate;
        ProductId = order.ProductId;
        Quantity= order.Quantity;
        Status = order.Status;
        Product = order.Product != null ? new ProductModel(order.Product) : null;
    }

    public Order ToEntity()
    {
        return new Order
        {
            OrderId = this.OrderId,
            OrderDate = this.OrderDate,
            ProductId = this.ProductId,
            Quantity = this.Quantity,
            Status = this.Status,
            Product = this.Product != null ? this.Product.ToEntity() : null
        };
    }
}

