using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface IStateRepository : IDisposable
    {
        Task CreateState(State state);
        Task CreateState(IEnumerable<State> states);
        Task<State> GetState(int stateId);
        Task<IEnumerable<State>> GetUserStates(int userId);
        Task SetStateName(int stateId, string stateName);
        Task SetStateCode(int stateId, int stateCode);
        Task SetStateExpirationTime(int stateId, int expirationTime);
        Task SetStateAction(int stateId, int actionId);
    }
}