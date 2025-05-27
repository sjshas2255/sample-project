using BusinessEntities;
using Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[AutoRegister]
public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Order>> GetOrders(string status = null, DateTime? orderDate = null) => _repository.GetOrders(status, orderDate);

    public Task<Order> GetOrderById(int id) => _repository.GetOrderById(id);

    public Task<Order> CreateOrder(Order order) => _repository.CreateOrder(order);

    public Task<Order> UpdateOrder(int id, Order order) => _repository.UpdateOrder(id, order);

    public Task<bool> DeleteOrder(int id) => _repository.DeleteOrder(id);
}