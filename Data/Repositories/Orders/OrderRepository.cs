using BusinessEntities;
using Common;
using Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[AutoRegister]
public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetOrders(string status = null, DateTime? orderDate = null)
    {
        var query = _context.Orders
            .Include(o => o.Product)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(o => o.Status.ToLower() == status.ToLower());
        }

        if (orderDate.HasValue)
        {
            // Compare only the date part
            var date = orderDate.Value.Date;
            query = query.Where(o => o.OrderDate.Date == date);
        }

        return await query.ToListAsync();
    }

    public async Task<Order> GetOrderById(int orderId)
    {
        return await _context.Orders
            .Include(o => o.Product)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task<Order> CreateOrder(Order order)
    {
        // Validation: OrderId must be greater than zero
        if (order.OrderId <= 0)
            throw new System.ArgumentException("OrderId must be greater than zero.", nameof(order.OrderId));

        // Check for existing order by OrderId
        var existing = await GetOrderById(order.OrderId);
        if (existing != null)
            throw new System.InvalidOperationException($"An order with OrderId {order?.OrderId} already exists.");

        var product =  await _context.Products.FindAsync(order.ProductId);

        // Check the product id is valid
        if (product == null)
            throw new System.InvalidOperationException($"Couldnt find a product with product id:{order.ProductId}.");

        // Check if enough stock available to order
        if (product.StockQuantity < order.Quantity)
            throw new System.InvalidOperationException("Not enough stock available.");

        // Adjust the stock accordingly
        product.StockQuantity -= order.Quantity;

        if(string.IsNullOrWhiteSpace(order.Status))
        {
            //In ideal scenario this could be a lookup or enum but handling with a string now
            order.Status = "Received";
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> UpdateOrder(int orderId, Order order)
    {
        // Validation: OrderId must be greater than zero
        if (orderId <= 0)
            throw new System.ArgumentException("OrderId must be greater than zero.", nameof(orderId));

        var existing = await GetOrderById(orderId);
        if (existing == null)
            throw new System.InvalidOperationException($"An order with OrderId {orderId} does not exist.");

        var product = await _context.Products.FindAsync(order.ProductId);
        if (product == null)
            throw new System.InvalidOperationException($"Product with Id {order.ProductId} does not exist.");


        // Adjust stock based on quantity difference
        int quantityDifference = order.Quantity - existing.Quantity;
        if (quantityDifference > 0 && product.StockQuantity < quantityDifference)
            throw new System.InvalidOperationException("Not enough stock available for update.");

        product.StockQuantity -= quantityDifference;

        existing.OrderDate = order.OrderDate;
        existing.ProductId = order.ProductId;
        existing.Quantity = order.Quantity;
        existing.Status = order.Status;

        await _context.SaveChangesAsync();
        return await GetOrderById(orderId);
    }

    public async Task<bool> DeleteOrder(int OrderId)
    {
        var order = await _context.Orders.FindAsync(OrderId);
        if (order == null)
            throw new System.InvalidOperationException($"An order with OrderId {OrderId} does not exist.");

        var product = await _context.Products.FindAsync(order.ProductId);
        if (product != null)
        {
            product.StockQuantity += order.Quantity;
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }
}