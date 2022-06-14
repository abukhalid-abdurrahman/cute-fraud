using System.Collections.Generic;
using System.Threading.Tasks;
using Fraud.Entities.Enums;

namespace Fraud.UseCase.EventListener
{
    public interface IEventListener
    {
        Task HandleEvent<T>(T eventEntity);
    }

    public class AmountAboveAverageEventListener : IEventListener
    {
        public async Task HandleEvent<T>(T eventEntity)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class OperationsAboveAverageEventEventListener : IEventListener
    {
        public async Task HandleEvent<T>(T eventEntity)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class MinimumIntervalEventEventListener : IEventListener
    {
        public async Task HandleEvent<T>(T eventEntity)
        {
            throw new System.NotImplementedException();
        }
    }

    public abstract class EventListenerStoreBase
    {
        public IDictionary<EventType, IEventListener> EventListeners;
    }

    public class EventListenerStore : EventListenerStoreBase
    {
        public new IDictionary<EventType, IEventListener> EventListeners;

        public EventListenerStore()
        {
            EventListeners = new Dictionary<EventType, IEventListener>()
            {
                { EventType.MinimumIntervalEvent, new MinimumIntervalEventEventListener() },
                { EventType.OperationsAboveAverageEvent, new OperationsAboveAverageEventEventListener() },
                { EventType.AmountAboveAverageEvent, new AmountAboveAverageEventListener() },
            };
        }
    }
}