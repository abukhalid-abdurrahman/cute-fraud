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
        
        public async Task<ReturnResult<bool>> CreateUser(Users user)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UsersRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            
            var returnResult = new ReturnResult<bool>();
            
            const string query = @"INSERT INTO users (user_name, api_key, callback) 
                                   VALUES (@UserName, @ApiKey, @CallBack);";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, user);
            if (rowsAffected < 0)
            {
                returnResult.Result = false;
                FaultHandler.HandleError(ref returnResult, $"User insertion failed, rows affected: {rowsAffected}");
            }
            else
                return ReturnResult<bool>.SuccessResult(true);

            return returnResult;
        }

        public async Task<ReturnResult<Users>> GetUser(int userId)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UsersRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            
            var returnResult = new ReturnResult<Users>();

            const string query = @"SELECT * FROM users WHERE id = @UserId;";
            var userById = await _dbConnection.QueryFirstOrDefaultAsync<Users>(query, new
            {
                UserId = userId
            });
            
            if (userById == null)
                FaultHandler.HandleWarning(ref returnResult, "User fetching is failed!", $"User with id {userId} not exist or could not be found!");
            else
                return ReturnResult<Users>.SuccessResult(userById);

            return returnResult;
        }

        public async Task<ReturnResult<Users>> GetUserByApiKey(string userApiKey)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UsersRepository));
            if(_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
            
            var returnResult = new ReturnResult<Users>();

            const string query = @"SELECT * FROM users WHERE api_key = @UserApiKey;";
            var userByApiKey = await _dbConnection.QueryFirstOrDefaultAsync<Users>(query, new
            {
                UserApiKey = userApiKey
            });

            if (userByApiKey == null)
                FaultHandler.HandleWarning(ref returnResult, "User fetching is failed!", $"User with api key {userApiKey} not exist or could not be found!");
            else
                return ReturnResult<Users>.SuccessResult(userByApiKey);
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

        ~UsersRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}