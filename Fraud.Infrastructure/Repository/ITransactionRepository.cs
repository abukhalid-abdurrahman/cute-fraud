using System;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface ITransactionRepository : IDisposable
    {
        public Task<ReturnResult<bool>> Create(Transaction entity);
        public Task<ReturnResult<Transaction[]>> FindByDateRange(string cardToken, DateTimeOffset dateFrom, DateTimeOffset dateTo);
        public Task<ReturnResult<Transaction[]>> FindLimit(string cardToken, int limit = 100);
    }
}