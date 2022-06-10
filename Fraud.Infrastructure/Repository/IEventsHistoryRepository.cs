using System;
using System.Threading.Tasks;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface IEventsHistoryRepository : IDisposable
    {
        Task<EventHistory> GetEventHistoryById(int eventHistoryId);
        Task CreateEventHistory(EventHistory eventHistory);
        Task SetEventHistoryOrderState(string orderExternalRef, int stateId);
        Task SetEventHistoryOrderEvent(string orderExternalRef, int eventId);
    }
}