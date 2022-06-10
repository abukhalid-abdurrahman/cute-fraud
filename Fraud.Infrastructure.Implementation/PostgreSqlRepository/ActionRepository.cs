using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Fraud.Concerns.Configurations;
using Fraud.Infrastructure.Repository;
using Npgsql;
using Action = Fraud.Entities.Models.Action;

namespace Fraud.Infrastructure.Implementation.PostgreSqlRepository
{
    public class ActionRepository : IActionRepository
    {
        private readonly PostgreSqlConfigurations _postgreSqlConfigurations;
        private readonly IDbConnection _dbConnection;
        private bool _isDisposed = false;

        public ActionRepository(PostgreSqlConfigurations postgreSqlConfigurations)
        {
            _postgreSqlConfigurations = postgreSqlConfigurations;
            _dbConnection = new NpgsqlConnection(postgreSqlConfigurations.ConnectionString);
        }
        
        public async Task<IEnumerable<Action>> GetAllActions()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ActionRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"SELECT * FROM actions;";
            return await _dbConnection.QueryAsync<Action>(query);
        }

        public async Task<IEnumerable<Action>> GetActionById(int actionId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ActionRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"SELECT * FROM actions WHERE id = @ActionId;";
            return await _dbConnection.QueryAsync<Action>(query, new { ActionId = actionId });
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

        ~ActionRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}