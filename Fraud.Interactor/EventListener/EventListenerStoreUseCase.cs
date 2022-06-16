using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.UseCase.EventListener;

namespace Fraud.Interactor.EventListener
{
    public class EventListenerStoreUseCase : IEventListenerStoreUseCase
    {
        private readonly IDictionary<EventType, IEventListenerUseCase> _eventListeners;

        public EventListenerStoreUseCase()
        {
            _eventListeners = new Dictionary<EventType, IEventListenerUseCase>()
            {
                { EventType.MinimumIntervalEvent, MinimumIntervalEventEventListenerUseCase.GetInstance() },
                { EventType.OperationsAboveAverageEvent, OperationsAboveAverageEventEventListenerUseCase.GetInstance() },
                { EventType.AmountAboveAverageEvent, AmountAboveAverageEventListenerUseCase.GetInstance() },
            };
        }
        
        public async Task RaiseEvent(EventType eventType, Order order)
        {
            if(!_eventListeners.ContainsKey(eventType))
                return;
            await _eventListeners[eventType].HandleEvent(order);
        }
    }
}