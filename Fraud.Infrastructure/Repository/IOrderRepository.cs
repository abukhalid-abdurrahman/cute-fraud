using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface IOrderRepository : IDisposable
    {
        Task<ReturnResult<bool>> CreateOrder(Order order);
        Task<ReturnResult<IEnumerable<Order>>> GetOrdersBySource(string source, DateTimeOffset dateFrom, DateTimeOffset dateTo, int limit = 100);
        Task<ReturnResult<IEnumerable<Order>>> GetOrdersByDestination(string destination, DateTimeOffset dateFrom, DateTimeOffset dateTo, int limit = 100);
    }
}