using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;

namespace Fraud.UseCase.EventListener
{
    public interface IEventListenerStoreUseCase
    {
        public Task<ReturnResult<bool>> RaiseEvent(EventType eventType, Order order);
    }
}