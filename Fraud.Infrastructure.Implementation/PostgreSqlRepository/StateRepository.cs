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
    public class StateRepository : IStateRepository
    {
        private readonly PostgreSqlConfigurations _postgreSqlConfigurations;
        private readonly IDbConnection _dbConnection;
        private bool _isDisposed = false;

        public StateRepository(PostgreSqlConfigurations postgreSqlConfigurations)
        {
            _postgreSqlConfigurations = postgreSqlConfigurations;
            _dbConnection = new NpgsqlConnection(postgreSqlConfigurations.ConnectionString);
        }
        
        public async Task<int> CreateState(State state)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"INSERT INTO states (user_id, state_name, state_code, expiration_time) 
                                   VALUES (@UserId, @StateName, @StateCode, @ExpirationTime) RETURNING id;";
            return await _dbConnection.ExecuteScalarAsync<int>(query, state);
        }

        public async Task CreateState(IEnumerable<State> states)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            using var transaction = _dbConnection.BeginTransaction();
            try
            {
                const string query = @"INSERT INTO states (user_id, state_name, state_code, expiration_time) 
                                   VALUES (@UserId, @StateName, @StateCode, @ExpirationTime);";
                await _dbConnection.ExecuteAsync(query, states, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<State> GetState(int stateId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"SELECT * FROM states WHERE id = @StateId;";
            return await _dbConnection.QueryFirstOrDefaultAsync<State>(query, new
            {
                StateId = stateId
            });
        }

        public async Task<IEnumerable<State>> GetUserStates(int userId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"SELECT * FROM states WHERE user_id = @UserId;";
            return await _dbConnection.QueryAsync<State>(query, new
            {
                UserId = userId
            });
        }

        public async Task SetStateName(int stateId, string stateName)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"UPDATE states 
                                   SET state_name = @StateName 
                                   WHERE id = @StateId;";
            await _dbConnection.ExecuteAsync(query, new
            {
                StateId = stateId,
                StateName = stateName
            });
        }

        public async Task SetStateCode(int stateId, int stateCode)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"UPDATE states 
                                   SET state_code = @StateCode 
                                   WHERE id = @StateId;";
            await _dbConnection.ExecuteAsync(query, new
            {
                StateId = stateId,
                StateCode = stateCode
            });
        }

        public async Task SetStateExpirationTime(int stateId, int expirationTime)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"UPDATE states 
                                   SET expiration_time = @ExpirationTime 
                                   WHERE id = @StateId;";
            await _dbConnection.ExecuteAsync(query, new
            {
                StateId = stateId,
                ExpirationTime = expirationTime
            });
        }

        public async Task SetStateAction(int stateId, int actionId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(OrderRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"INSERT INTO actions_states (action_id, state_id) VALUES (@ActionId, @StateId);";
            await _dbConnection.ExecuteAsync(query, new
            {
                ActionId = actionId,
                StateId = stateId
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

        ~StateRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}