using System.Threading.Tasks;
using Fraud.Entities.Models;
using Fraud.UseCase.EventListener;

namespace Fraud.Interactor.EventListener
{
    public sealed class MinimumIntervalEventEventListener : IEventListener
    {
        private static readonly object Locker = new object();
        private static MinimumIntervalEventEventListener _instance = null;
        public static IEventListener GetInstance()
        {
            lock (Locker)
            {
                return _instance ??= new MinimumIntervalEventEventListener();
            }
        }

        public async Task HandleEvent(Order orderEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}