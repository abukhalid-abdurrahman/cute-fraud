using System;
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
        
        public async Task<ReturnResult<int>> CreateScenario(Scenario scenario)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ScenarioRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"INSERT INTO scenarios (user_id, rule) 
                                   VALUES (@UserId, CAST(@Rule as json)) RETURNING id;";
            return ReturnResult<int>.SuccessResult(await _dbConnection.ExecuteScalarAsync<int>(query, scenario));
        }

        public async Task<ReturnResult<bool>> SetScenarioRule(int scenarioId, string scenarioRule)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ScenarioRepository));

            if (_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var returnResult = new ReturnResult<bool>();
            const string query = @"UPDATE scenarios 
                                   SET rule = @ScenarioRule 
                                   WHERE id = @ScenarioId;";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, new
            {
                ScenarioRule = scenarioRule,
                ScenarioId = scenarioId
            });

            if (rowsAffected < 0)
            {
                returnResult.Result = false;
                FaultHandler.HandleError(ref returnResult, $"Scenario updating failed, rows affected: {rowsAffected}");
            }
            else
                return ReturnResult<bool>.SuccessResult(true);
            return returnResult;
        }

        public async Task<ReturnResult<string>> GetScenarioRule(int scenarioId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(OrderRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var returnResult = new ReturnResult<string>();
            
            const string query = @"SELECT rule FROM scenarios WHERE id = @ScenarioId;";
            var scenarioRule = await _dbConnection.QueryFirstOrDefaultAsync<string>(query, new
            {
                ScenarioId = scenarioId
            });
            
            if (string.IsNullOrEmpty(scenarioRule))
                FaultHandler.HandleError(ref returnResult, $"Scenario rule with id {scenarioId} is null or was not found!");
            else
                return ReturnResult<string>.SuccessResult(scenarioRule);
            return returnResult;
        }

        public async Task<ReturnResult<bool>> DeleteScenario(int scenarioId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(OrderRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var returnResult = new ReturnResult<bool>();
            
            const string query = @"DELETE FROM scenarios WHERE id = @ScenarioId;";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, new
            {
                ScenarioId = scenarioId
            });
            if (rowsAffected < 0)
            {
                returnResult.Result = false;
                FaultHandler.HandleError(ref returnResult, $"Scenario deleting failed, rows affected: {rowsAffected}");
            }
            else
                return ReturnResult<bool>.SuccessResult(true);
            return returnResult;
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