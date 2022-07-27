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
        
        public async Task<ReturnResult<EventHistory>> GetEventHistoryById(int eventHistoryId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(EventsHistoryRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var result = new ReturnResult<EventHistory>();
            
            const string query = @"SELECT * FROM events_history WHERE id = @EventHistoryId;";
            var eventHistoryById = await _dbConnection.QueryFirstOrDefaultAsync<EventHistory>(query, new
            {
                EventHistoryId = eventHistoryId
            });
            if (eventHistoryById == null)
                FaultHandler.HandleWarning(ref result, "Event history not found!", $"Event history with id: {eventHistoryId} not found!");
            else
                return ReturnResult<EventHistory>.SuccessResult(eventHistoryById);
            
            return result;
        }

        public async Task<ReturnResult<bool>> CreateEventHistory(EventHistory eventHistory)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(EventsHistoryRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var result = new ReturnResult<bool>();
            var errorMessageTemplate = "Event history insertion failed! Rows affected: {0}, order_external_ref: {1}";
            
            const string query = @"INSERT INTO events_history (order_external_ref, scenario_id, state_id, event_id) 
                                   VALUES (@OrderExternalRef, @ScenarioId, @StateId, @EventId);";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, eventHistory);

            if (rowsAffected <= 0)
                FaultHandler.HandleError(ref result, string.Format(errorMessageTemplate, rowsAffected, eventHistory.OrderExternalRef));
            else
                return ReturnResult<bool>.SuccessResult(true);
            return result;
        }

        public async Task<ReturnResult<bool>> SetEventHistoryOrderState(string orderExternalRef, int stateId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(EventsHistoryRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            
            var result = new ReturnResult<bool>();
            var errorMessageTemplate = "Event history updating failed in method SetEventHistoryOrderState! Rows affected: {0}, order_external_ref: {1}";
            
            const string query = @"UPDATE events_history 
                                   SET state_id = @StateId
                                   WHERE order_external_ref = @OrderExternalRef;";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, new
            {
                OrderExternalRef = orderExternalRef,
                StateId = stateId
            });
            if (rowsAffected <= 0)
                FaultHandler.HandleError(ref result, string.Format(errorMessageTemplate, rowsAffected, orderExternalRef));
            else
                return ReturnResult<bool>.SuccessResult(true);

            return result;
        }

        public async Task<ReturnResult<bool>> SetEventHistoryOrderEvent(string orderExternalRef, int eventId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(EventsHistoryRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            
            var result = new ReturnResult<bool>();
            var errorMessageTemplate = "Event history updating failed in method SetEventHistoryOrderEvent! Rows affected: {0}, order_external_ref: {1}";
            
            const string query = @"UPDATE events_history 
                                   SET event_id = @EventId
                                   WHERE order_external_ref = @OrderExternalRef;";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, new
            {
                OrderExternalRef = orderExternalRef,
                EventId = eventId
            });
            if (rowsAffected <= 0)
                FaultHandler.HandleError(ref result, string.Format(errorMessageTemplate, rowsAffected, orderExternalRef));
            else
                return ReturnResult<bool>.SuccessResult(true);
            return result;
        }

        public async Task<ReturnResult<bool>> SetEventHistoryOrderStateAndEvent(string orderExternalRef, int eventId, int actionId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(EventsHistoryRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            
            var result = new ReturnResult<bool>();
            var errorMessageTemplate = "Event history updating failed in method SetEventHistoryOrderStateAndEvent! Rows affected: {0}, order_external_ref: {1}";
            
            const string query = @"UPDATE events_history SET 
                                   event_id = @EventId,
                                   action_id = @ActionId
                                   WHERE order_external_ref = @OrderExternalRef;";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, new
            {
                OrderExternalRef = orderExternalRef,
                EventId = eventId,
                ActionId = actionId
            });
            if (rowsAffected <= 0)
                FaultHandler.HandleError(ref result, string.Format(errorMessageTemplate, rowsAffected, orderExternalRef));
            else
                return ReturnResult<bool>.SuccessResult(true);
            return result;
        }

        public async Task<ReturnResult<EventHistory>> GetEventHistoryByOrderId(string orderExternalRef)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(EventsHistoryRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var result = new ReturnResult<EventHistory>();
            
            const string query = @"SELECT * FROM events_history WHERE order_external_ref = @OrderExternalRef;";
            var eventHistoryByOrder = await _dbConnection.QueryFirstOrDefaultAsync<EventHistory>(query, new
            {
                OrderExternalRef = orderExternalRef
            });
            if (eventHistoryByOrder == null)
                FaultHandler.HandleWarning(ref result, "Event history not found!", $"Event history with order_external_Ref: {orderExternalRef} not found!");
            else
                return ReturnResult<EventHistory>.SuccessResult(eventHistoryByOrder);
            
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

        ~EventsHistoryRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}