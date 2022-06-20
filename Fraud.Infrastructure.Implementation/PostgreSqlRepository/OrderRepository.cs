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
    public class OrderRepository : IOrderRepository
    {
        private readonly PostgreSqlConfigurations _postgreSqlConfigurations;
        private readonly IDbConnection _dbConnection;
        private bool _isDisposed = false;

        public OrderRepository(PostgreSqlConfigurations postgreSqlConfigurations)
        {
            _postgreSqlConfigurations = postgreSqlConfigurations;
            _dbConnection = new NpgsqlConnection(postgreSqlConfigurations.ConnectionString);
        }

        public async Task<ReturnResult<bool>> CreateOrder(Order order)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(OrderRepository));
            
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var returnResult = new ReturnResult<bool>();

            const string query = @"INSERT INTO orders (external_ref, user_id, amount, source, destination) 
                                   VALUES (@ExternalRef, @UserId, @Amount, @Source, @Destination) RETURNING id;";
            var rowsAffected = await _dbConnection.ExecuteScalarAsync<int>(query, order);
            if (rowsAffected < 1)
            {
                returnResult.Result = false;
                FaultHandler.HandleError(ref returnResult, $"Order insertion failed! Rows affected: {rowsAffected}");
            }
            else
                return ReturnResult<bool>.SuccessResult(true);
            return returnResult;
        }

        public async Task<ReturnResult<IEnumerable<Order>>> GetOrdersBySource(string source, DateTimeOffset dateFrom, DateTimeOffset dateTo, int limit = 100)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(OrderRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            
            var returnResult = new ReturnResult<IEnumerable<Order>>();
            
            const string query = @"SELECT * FROM orders WHERE source = @Source AND date_created BETWEEN @DateFrom AND @DateTo LIMIT @FetchLimit;";
            var ordersBySource = await _dbConnection.QueryAsync<Order>(query, new
            {
                Source = source,
                DateFrom = dateFrom,
                DateTo = dateTo,
                FetchLimit = limit
            });

            if (!ordersBySource.Any())
                FaultHandler.HandleWarning(ref returnResult, "Orders by source could not be found!", $"Orders by source: {source} not exist!");
            else
                return ReturnResult<IEnumerable<Order>>.SuccessResult(ordersBySource);

            return returnResult;
        }

        public async Task<ReturnResult<IEnumerable<Order>>> GetOrdersByDestination(string destination, DateTimeOffset dateFrom, DateTimeOffset dateTo, int limit = 100)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(OrderRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            
            var returnResult = new ReturnResult<IEnumerable<Order>>();
            
            const string query = @"SELECT * FROM orders WHERE destination = @Destination AND date_created BETWEEN @DateFrom AND @DateTo LIMIT @FetchLimit;";
            var ordersByDestination = await _dbConnection.QueryAsync<Order>(query, new
            {
                Destination = destination,
                DateFrom = dateFrom,
                DateTo = dateTo,
                FetchLimit = limit
            });
            
            if (!ordersByDestination.Any())
                FaultHandler.HandleWarning(ref returnResult, "Orders by destination could not be found!", $"Orders by destination: {destination} not exist!");
            else
                return ReturnResult<IEnumerable<Order>>.SuccessResult(ordersByDestination);

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

        ~OrderRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}