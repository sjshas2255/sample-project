using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrders(string status = null, DateTime? orderDate = null);
    Task<Order> GetOrderById(int id);
    Task<Order> CreateOrder(Order order);
    Task<Order> UpdateOrder(int id, Order order);
    Task<bool> DeleteOrder(int id);
}