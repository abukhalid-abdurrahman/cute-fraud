using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface IOrderRepository : IDisposable
    {
        Task CreateOrder(Order order);
        Task<IEnumerable<Order>> GetOrdersBySource(string source, DateTimeOffset dateFrom, DateTimeOffset dateTo, int limit = 100);
        Task<IEnumerable<Order>> GetOrdersByDestination(string destination, DateTimeOffset dateFrom, DateTimeOffset dateTo, int limit = 100);
    }
}