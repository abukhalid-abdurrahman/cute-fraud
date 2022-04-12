using System.Threading.Tasks;
using Fraud.UseCase.MessageBroking;

namespace Fraud.Interactor.MessageBroking
{
    public class MessageNotifierInteractor : IMessageNotifier
    {
        /// <summary>
        /// Sends message to message broker to notify other service about raised event.
        /// </summary>
        /// <param name="queueName">Queue name that will accept message</param>
        /// <param name="message">Message that need to sent to message broker</param>
        public async Task Notify(string queueName, string message)
        {
            
        }
    }
}