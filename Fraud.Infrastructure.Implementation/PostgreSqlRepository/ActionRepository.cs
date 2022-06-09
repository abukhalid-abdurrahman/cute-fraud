using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Action>> GetActionById(int actionId)
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

        ~ActionRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}