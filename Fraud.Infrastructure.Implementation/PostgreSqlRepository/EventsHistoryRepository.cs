using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
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
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(EventsHistoryRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"SELECT * FROM events_history WHERE id = @EventHistoryId;";
            return await _dbConnection.QueryFirstOrDefaultAsync(query, new
            {
                EventHistoryId = eventHistoryId
            });
        }

        public async Task CreateEventHistory(EventHistory eventHistory)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(EventsHistoryRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"INSERT INTO events_history (order_external_ref, scenario_id, state_id, event_id) 
                                   VALUES (@OrderExternalRef, @ScenarioId, @StateId, @EventId);";
            await _dbConnection.ExecuteAsync(query, eventHistory);
        }

        public async Task SetEventHistoryOrderState(string orderExternalRef, int stateId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(EventsHistoryRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"UPDATE events_history 
                                   SET state_id = @StateId
                                   WHERE order_external_ref = @OrderExternalRef;";
            await _dbConnection.ExecuteAsync(query, new
            {
                OrderExternalRef = orderExternalRef,
                StateId = stateId
            });
        }

        public async Task SetEventHistoryOrderEvent(string orderExternalRef, int eventId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(EventsHistoryRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"UPDATE events_history 
                                   SET event_id = @EventId
                                   WHERE order_external_ref = @OrderExternalRef;";
            await _dbConnection.ExecuteAsync(query, new
            {
                OrderExternalRef = orderExternalRef,
                EventId = eventId
            });
        }

        private void ReleaseUnmanagedResources()
        {
            if(_dbConnection.State != ConnectionState.Closed)
                _dbConnection.Close();
            _dbConnection.Dispose();
            _isDisposed = true;
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