using System.Threading.Tasks;
using Fraud.Entities.Models;
using Fraud.UseCase.EventListener;

namespace Fraud.Interactor.EventListener
{
    public sealed class OperationsAboveAverageEventEventListenerUseCase : IEventListenerUseCase
    {
        private static readonly object Locker = new object();
        private static OperationsAboveAverageEventEventListenerUseCase _instance = null;
        public static IEventListenerUseCase GetInstance()
        {
            lock (Locker)
            {
                return _instance ??= new OperationsAboveAverageEventEventListenerUseCase();
            }
        }

        public async Task HandleEvent(Order orderEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}