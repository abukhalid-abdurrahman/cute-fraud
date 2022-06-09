using System;
using System.Data;
using System.Threading.Tasks;
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
        
        public async Task CreateScenario(Scenario scenario)
        {
            throw new NotImplementedException();
        }

        public async Task SetScenarioRule(int scenarioId, string scenarioRule)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetScenarioRule(int scenarioId, int userId)
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

        ~ScenarioRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}