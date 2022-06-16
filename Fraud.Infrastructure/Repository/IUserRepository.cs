using System;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface IUserRepository : IDisposable
    {
        Task<ReturnResult<bool>> CreateUser(Users user);
        Task<ReturnResult<Users>> GetUser(int userId);
        Task<ReturnResult<Users>> GetUserByApiKey(string userApiKey);
    }
}