using System;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface IEventsHistoryRepository : IDisposable
    {
        Task<ReturnResult<EventHistory>> GetEventHistoryById(int eventHistoryId);
        Task<ReturnResult<bool>> CreateEventHistory(EventHistory eventHistory);
        Task<ReturnResult<bool>> SetEventHistoryOrderState(string orderExternalRef, int stateId);
        Task<ReturnResult<bool>> SetEventHistoryOrderEvent(string orderExternalRef, int eventId);
    }
}