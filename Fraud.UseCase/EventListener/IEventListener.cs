using System.Threading.Tasks;
using Fraud.Entities.Models;

namespace Fraud.UseCase.EventListener
{
    public interface IEventListener
    {
        Task HandleEvent(Order orderEntity);
    }
}