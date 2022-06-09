using System;
using System.Threading.Tasks;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface IUserRepository : IDisposable
    {
        Task CreateUser(Users user);
        Task<Users> GetUser(int userId);
        Task<Users> GetUserByApiKey(string userApiKey);
    }
}