using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.UseCase.EventListener;

namespace Fraud.Interactor.EventListener
{
    public class EventListenerStore : IEventListenerStore
    {
        private readonly IDictionary<EventType, IEventListener> _eventListeners;

        public EventListenerStore()
        {
            _eventListeners = new Dictionary<EventType, IEventListener>()
            {
                { EventType.MinimumIntervalEvent, MinimumIntervalEventEventListener.GetInstance() },
                { EventType.OperationsAboveAverageEvent, OperationsAboveAverageEventEventListener.GetInstance() },
                { EventType.AmountAboveAverageEvent, AmountAboveAverageEventListener.GetInstance() },
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