using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Infrastructure.Repository;

namespace Fraud.Infrastructure.Implementation.Repository.Neo4J
{
    public class Neo4JFraudRepository<TEntity> : IFraudRepository<TEntity>
    {
        public Neo4JFraudRepository()
        {
            
        }

        public async Task Create(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<TEntity>> FindByDateRange(DateTime dateFrom, DateTime dateTo)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<TEntity>> FindLimit(int limit = 100)
        {
            throw new NotImplementedException();
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~Neo4JFraudRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}