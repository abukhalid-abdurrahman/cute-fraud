using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Models;

namespace Fraud.UseCase.EventListener
{
    public interface IEventListenerUseCase
    {
        Task<ReturnResult<bool>> HandleEvent(Order orderEntity);
    }
}