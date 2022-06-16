using System.Threading.Tasks;
using Fraud.Entities.Models;
using Fraud.UseCase.EventListener;

namespace Fraud.Interactor.EventListener
{
    public sealed class MinimumIntervalEventEventListenerUseCase : IEventListenerUseCase
    {
        private static readonly object Locker = new object();
        private static MinimumIntervalEventEventListenerUseCase _instance = null;
        public static IEventListenerUseCase GetInstance()
        {
            lock (Locker)
            {
                return _instance ??= new MinimumIntervalEventEventListenerUseCase();
            }
        }

        public async Task HandleEvent(Order orderEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}