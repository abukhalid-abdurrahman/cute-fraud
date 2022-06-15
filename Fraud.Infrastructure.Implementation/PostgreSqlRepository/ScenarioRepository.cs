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
    public class ScenarioRepository : IScenarioRepository
    {
        private readonly PostgreSqlConfigurations _postgreSqlConfigurations;
        private readonly IDbConnection _dbConnection;
        private bool _isDisposed = false;

        public ScenarioRepository(PostgreSqlConfigurations postgreSqlConfigurations)
        {
            _postgreSqlConfigurations = postgreSqlConfigurations;
            _dbConnection = new NpgsqlConnection(postgreSqlConfigurations.ConnectionString);
        }
        
        public async Task<int> CreateScenario(Scenario scenario)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ScenarioRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"INSERT INTO scenarios (user_id, rule) 
                                   VALUES (@UserId, CAST(@Rule as json)) RETURNING id;";
            return await _dbConnection.ExecuteScalarAsync<int>(query, scenario);
        }

        public async Task SetScenarioRule(int scenarioId, string scenarioRule)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ScenarioRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"UPDATE scenarios 
                                   SET rule = @ScenarioRule 
                                   WHERE id = @ScenarioId;";
            await _dbConnection.ExecuteAsync(query, new
            {
                ScenarioRule = scenarioRule,
                ScenarioId = scenarioId
            });
        }

        public async Task<string> GetScenarioRule(int scenarioId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(OrderRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"SELECT * FROM scenarios WHERE id = @ScenarioId;";
            return await _dbConnection.QueryFirstOrDefaultAsync<string>(query, new
            {
                ScenarioId = scenarioId
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

        ~ScenarioRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}