using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fraud.Infrastructure.Repository
{
    public interface IFraudRepository<TEntity> : IDisposable
    {
        public Task Create(TEntity entity);
        public Task<IReadOnlyList<TEntity>> FindByDateRange(DateTime dateFrom, DateTime dateTo);
        public Task<IReadOnlyList<TEntity>> FindLimit(int limit = 100);
    }
}