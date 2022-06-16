using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Fraud.Concerns;
using Fraud.Concerns.Configurations;
using Fraud.Concerns.FaultHandling;
using Fraud.Entities.Models;
using Fraud.Infrastructure.Repository;
using Npgsql;

namespace Fraud.Infrastructure.Implementation.PostgreSqlRepository
{
    public class EventRepository : IEventRepository
    {
        private readonly PostgreSqlConfigurations _postgreSqlConfigurations;
        private readonly IDbConnection _dbConnection;
        private bool _isDisposed = false;

        public EventRepository(PostgreSqlConfigurations postgreSqlConfigurations)
        {
            _postgreSqlConfigurations = postgreSqlConfigurations;
            _dbConnection = new NpgsqlConnection(postgreSqlConfigurations.ConnectionString);
        }
        
        public async Task<ReturnResult<IEnumerable<Events>>> GetAllEvents()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(EventRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            
            var result = new ReturnResult<IEnumerable<Events>>();
            
            const string query = @"SELECT * FROM events;";
            var allEvents =  await _dbConnection.QueryAsync<Events>(query);
            if(allEvents == null)
                FaultHandler.HandleWarning(ref result, "Events list are empty!", "Pre-built event list are not exist!");
            else
                return ReturnResult<IEnumerable<Events>>.SuccessResult(allEvents);

            return result;
        }

        public async Task<ReturnResult<Events>> GetEventById(int eventId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(EventRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var result = new ReturnResult<Events>();
            
            const string query = @"SELECT * FROM events WHERE id = @EventId;";
            var eventEntity = await _dbConnection.QueryFirstOrDefaultAsync<Events>(query, new { EventId = eventId });
            if(eventEntity == null)
                FaultHandler.HandleWarning(ref result, "Entity not found!", $"Pre-built entity with id {eventId} not found!");
            else
                return ReturnResult<Events>.SuccessResult(eventEntity);

            return result;
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

        ~EventRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}