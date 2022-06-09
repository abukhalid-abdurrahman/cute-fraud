using System;
using System.Data;
using System.Threading.Tasks;
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
        
        public async Task CreateState(State state)
        {
            throw new NotImplementedException();
        }

        public async Task<State> GetState(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task SetStateName(int stateId, string stateName)
        {
            throw new NotImplementedException();
        }

        public async Task SetStateCode(int stateId, int stateCode)
        {
            throw new NotImplementedException();
        }

        public async Task SetStateExpirationTime(int stateId, int expirationTime)
        {
            throw new NotImplementedException();
        }

        public async Task SetStateAction(int stateId, int actionId)
        {
            throw new NotImplementedException();
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
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