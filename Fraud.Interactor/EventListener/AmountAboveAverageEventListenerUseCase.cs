using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Models;
using Fraud.UseCase.EventListener;

namespace Fraud.Interactor.EventListener
{
    public sealed class AmountAboveAverageEventListenerUseCase : IEventListenerUseCase
    {
        private static readonly object Locker = new object();
        private static AmountAboveAverageEventListenerUseCase _instance = null;
        public static IEventListenerUseCase GetInstance()
        {
            lock (Locker)
            {
                return _instance ??= new AmountAboveAverageEventListenerUseCase();
            }
        }
        
        public async Task<ReturnResult<bool>> HandleEvent(Order orderEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}