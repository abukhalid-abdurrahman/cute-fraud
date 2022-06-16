using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Fraud.Concerns;
using Fraud.Concerns.Configurations;
using Fraud.Concerns.FaultHandling;
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
        
        public async Task<ReturnResult<IEnumerable<Action>>> GetAllActions()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ActionRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            var result = new ReturnResult<IEnumerable<Action>>();
            
            const string query = @"SELECT * FROM actions;";
            var allActions =  await _dbConnection.QueryAsync<Action>(query);
            if (!allActions.Any())
                FaultHandler.HandleWarning(ref result, "Actions list are empty!", "Pre-built action list are not exist!");
            else
                return ReturnResult<IEnumerable<Action>>.SuccessResult(allActions);
            
            return result;
        }

        public async Task<ReturnResult<Action>> GetActionById(int actionId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ActionRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            
            var result = new ReturnResult<Action>();

            const string query = @"SELECT * FROM actions WHERE id = @ActionId;";
            var action = await _dbConnection.QueryFirstOrDefaultAsync<Action>(query, new { ActionId = actionId });
            if(action == null)
                FaultHandler.HandleWarning(ref result, "Action not exist!", $"Pre-built action with id {actionId} not found!");
            else
                return ReturnResult<Action>.SuccessResult(action);

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

        ~ActionRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}