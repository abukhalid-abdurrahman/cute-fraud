using System.Threading.Tasks;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;

namespace Fraud.UseCase.EventListener
{
    public interface IEventListenerStore
    {
        public Task RaiseEvent(EventType eventType, Order order);
    }
}