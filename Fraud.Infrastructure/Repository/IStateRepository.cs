using System;
using System.Threading.Tasks;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface IStateRepository : IDisposable
    {
        Task CreateState(State state);
        Task<State> GetState(int userId);
        Task SetStateName(int stateId, string stateName);
        Task SetStateCode(int stateId, int stateCode);
        Task SetStateExpirationTime(int stateId, int expirationTime);
        Task SetStateAction(int stateId, int actionId);
    }
}