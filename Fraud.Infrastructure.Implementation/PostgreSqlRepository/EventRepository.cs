using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Fraud.Concerns.Configurations;
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
        
        public async Task<IEnumerable<Events>> GetAllEvents()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(EventRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"SELECT * FROM events;";
            return await _dbConnection.QueryAsync<Events>(query);
        }

        public async Task<IEnumerable<Events>> GetEventById(int eventId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(EventRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"SELECT * FROM actions WHERE id = @ActionId;";
            return await _dbConnection.QueryAsync<Events>(query, new { EventId = eventId });
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