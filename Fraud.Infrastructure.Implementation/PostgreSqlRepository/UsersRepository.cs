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
    public class UsersRepository : IUserRepository
    {
        private readonly PostgreSqlConfigurations _postgreSqlConfigurations;
        private readonly IDbConnection _dbConnection;
        private bool _isDisposed = false;

        public UsersRepository(PostgreSqlConfigurations postgreSqlConfigurations)
        {
            _postgreSqlConfigurations = postgreSqlConfigurations;
            _dbConnection = new NpgsqlConnection(postgreSqlConfigurations.ConnectionString);
        }
        
        public async Task CreateUser(Users user)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UsersRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            
            const string query = @"INSERT INTO users (user_name, api_key, callback) 
                                   VALUES (@UserName, @ApiKey, @CallBack);";
            await _dbConnection.ExecuteAsync(query, user);
        }

        public async Task<Users> GetUser(int userId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UsersRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"SELECT * FROM users WHERE id = @UserId;";
            return await _dbConnection.QueryFirstOrDefaultAsync<Users>(query, new
            {
                UserId = userId
            });
        }

        public async Task<Users> GetUserByApiKey(string userApiKey)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UsersRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            const string query = @"SELECT * FROM users WHERE api_key = @UserApiKey;";
            return await _dbConnection.QueryFirstOrDefaultAsync<Users>(query, new
            {
                UserApiKey = userApiKey
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

        ~UsersRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}