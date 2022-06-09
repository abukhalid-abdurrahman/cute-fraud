using System;
using System.Data;
using System.Threading.Tasks;
using Fraud.Concerns.Configurations;
using Fraud.Entities.Models;
using Fraud.Infrastructure.Repository;
using Npgsql;

namespace Fraud.Infrastructure.Implementation.PostgreSqlRepository
{
    public class EventsHistoryRepository : IEventsHistoryRepository
    {
        private readonly PostgreSqlConfigurations _postgreSqlConfigurations;
        private readonly IDbConnection _dbConnection;
        private bool _isDisposed = false;

        public EventsHistoryRepository(PostgreSqlConfigurations postgreSqlConfigurations)
        {
            _postgreSqlConfigurations = postgreSqlConfigurations;
            _dbConnection = new NpgsqlConnection(postgreSqlConfigurations.ConnectionString);
        }
        
        public async Task<EventHistory> GetEventHistoryById(int eventHistoryId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateEventHistory(EventHistory eventHistory)
        {
            throw new NotImplementedException();
        }

        public async Task SetEventHistoryOrderState(string orderExternalRef, int stateId)
        {
            throw new NotImplementedException();
        }

        public async Task SetEventHistoryOrderEvent(string orderExternalRef, int stateId)
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

        ~EventsHistoryRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}