using System.Threading.Tasks;
using Fraud.Entities.Models;
using Fraud.UseCase.EventListener;

namespace Fraud.Interactor.EventListener
{
    public sealed class AmountAboveAverageEventListener : IEventListener
    {
        private static readonly object Locker = new object();
        private static AmountAboveAverageEventListener _instance = null;
        public static IEventListener GetInstance()
        {
            lock (Locker)
            {
                return _instance ??= new AmountAboveAverageEventListener();
            }
        }
        
        public async Task HandleEvent(Order orderEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}