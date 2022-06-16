using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface IStateRepository : IDisposable
    {
        Task<ReturnResult<int>> CreateState(State state);
        Task<ReturnResult<bool>> CreateState(IEnumerable<State> states);
        Task<ReturnResult<State>> GetState(int stateId);
        Task<ReturnResult<IEnumerable<State>>> GetUserStates(int userId);
        Task<ReturnResult<bool>> SetStateName(int stateId, string stateName);
        Task<ReturnResult<bool>> SetStateCode(int stateId, int stateCode);
        Task<ReturnResult<bool>> SetStateExpirationTime(int stateId, int expirationTime);
        Task<ReturnResult<bool>> SetStateAction(int stateId, int actionId);
    }
}