using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public class StateRepository : IStateRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly PostgreSqlConfigurations _postgreSqlConfigurations;
        private bool _isDisposed;

        public StateRepository(PostgreSqlConfigurations postgreSqlConfigurations)
        {
            _postgreSqlConfigurations = postgreSqlConfigurations;
            _dbConnection = new NpgsqlConnection(postgreSqlConfigurations.ConnectionString);
        }

        public async Task<ReturnResult<int>> CreateState(State state)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateRepository));

            if (_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var returnResult = new ReturnResult<int>();
            const string query = @"INSERT INTO states (user_id, state_name, state_code, expiration_time) 
                                   VALUES (@UserId, @StateName, @StateCode, @ExpirationTime) RETURNING id;";
            var stateId = await _dbConnection.ExecuteScalarAsync<int>(query, state);

            if (stateId <= 0)
                FaultHandler.HandleError(ref returnResult, "State insertion failed!");
            else
                return ReturnResult<int>.SuccessResult(stateId);

            return returnResult;
        }

        public async Task<ReturnResult<bool>> CreateState(IEnumerable<State> states)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateRepository));

            if (_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var returnResult = new ReturnResult<bool>();
            var errorMessageTemplate = "Error was occurred while executing the transaction! Reason: {0}";

            using var transaction = _dbConnection.BeginTransaction();
            try
            {
                const string query = @"INSERT INTO states (user_id, state_name, state_code, expiration_time) 
                                   VALUES (@UserId, @StateName, @StateCode, @ExpirationTime);";
                await _dbConnection.ExecuteAsync(query, states, transaction);
                transaction.Commit();

                returnResult.Result = true;
            }
            catch (Exception e)
            {
                transaction.Rollback();

                returnResult.Result = false;
                FaultHandler.HandleError(ref returnResult, e, string.Format(errorMessageTemplate, e.Message),
                    e.StackTrace);
            }

            return returnResult;
        }

        public async Task<ReturnResult<State>> GetState(int stateId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateRepository));

            if (_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var returnResult = new ReturnResult<State>();

            const string query = @"SELECT * FROM states WHERE id = @StateId;";
            var stateEntity = await _dbConnection.QueryFirstOrDefaultAsync<State>(query, new
            {
                StateId = stateId
            });

            if (stateEntity == null)
                FaultHandler.HandleWarning(ref returnResult, "State not found!", $"State with id {stateId} not exist!");
            else
                return ReturnResult<State>.SuccessResult(stateEntity);

            return returnResult;
        }

        public async Task<ReturnResult<IEnumerable<State>>> GetUserStates(int userId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateRepository));

            if (_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var returnResult = new ReturnResult<IEnumerable<State>>();

            const string query = @"SELECT * FROM states WHERE user_id = @UserId;";
            var userStatesEntities = await _dbConnection.QueryAsync<State>(query, new
            {
                UserId = userId
            });

            if (!userStatesEntities.Any())
                FaultHandler.HandleWarning(ref returnResult, "User have not states!",
                    $"User with id {userId} have not states!");
            else
                return ReturnResult<IEnumerable<State>>.SuccessResult(userStatesEntities);

            return returnResult;
        }

        public async Task<ReturnResult<bool>> SetStateName(int stateId, string stateName)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateRepository));

            if (_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var returnResult = new ReturnResult<bool>();

            const string query = @"UPDATE states 
                                   SET state_name = @StateName 
                                   WHERE id = @StateId;";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, new
            {
                StateId = stateId,
                StateName = stateName
            });

            if (rowsAffected <= 0)
                FaultHandler.HandleWarning(ref returnResult, "Updating state name failed!",
                    $"{rowsAffected} Rows affected while updating state: {stateId}!");
            else
                return ReturnResult<bool>.SuccessResult(true);

            return returnResult;
        }

        public async Task<ReturnResult<bool>> SetStateCode(int stateId, int stateCode)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateRepository));

            if (_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var returnResult = new ReturnResult<bool>();

            const string query = @"UPDATE states 
                                   SET state_code = @StateCode 
                                   WHERE id = @StateId;";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, new
            {
                StateId = stateId,
                StateCode = stateCode
            });

            if (rowsAffected <= 0)
                FaultHandler.HandleWarning(ref returnResult, "Updating state code failed!",
                    $"{rowsAffected} Rows affected while updating state: {stateId}!");
            else
                return ReturnResult<bool>.SuccessResult(true);

            return returnResult;
        }

        public async Task<ReturnResult<bool>> SetStateExpirationTime(int stateId, int expirationTime)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(StateRepository));

            if (_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var returnResult = new ReturnResult<bool>();

            const string query = @"UPDATE states 
                                   SET expiration_time = @ExpirationTime 
                                   WHERE id = @StateId;";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, new
            {
                StateId = stateId,
                ExpirationTime = expirationTime
            });

            if (rowsAffected <= 0)
                FaultHandler.HandleWarning(ref returnResult, "Updating state expiration time failed!",
                    $"{rowsAffected} Rows affected while updating state: {stateId}!");
            else
                return ReturnResult<bool>.SuccessResult(true);

            return returnResult;
        }

        public async Task<ReturnResult<bool>> SetStateAction(int stateId, int actionId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(OrderRepository));
            if (_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var returnResult = new ReturnResult<bool>();

            const string query = @"INSERT INTO actions_states (action_id, state_id) VALUES (@ActionId, @StateId);";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, new
            {
                ActionId = actionId,
                StateId = stateId
            });

            if (rowsAffected <= 0)
                FaultHandler.HandleWarning(ref returnResult, "Insertion into actions_states failed!",
                    $"{rowsAffected} Rows affected while inserting: {stateId}, {actionId}!");
            else
                return ReturnResult<bool>.SuccessResult(true);

            return returnResult;
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        private void ReleaseUnmanagedResources()
        {
            if (_dbConnection.State != ConnectionState.Closed)
                _dbConnection.Close();
            _dbConnection.Dispose();
            _isDisposed = true;
        }

        ~StateRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}