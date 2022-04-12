using System.Threading.Tasks;

namespace Fraud.UseCase.MessageBroking
{
    public interface IMessageNotifier
    {
        /// <summary>
        /// Sends message to message broker to notify other service about raised event.
        /// </summary>
        /// <param name="queueName">Queue name that will accept message</param>
        /// <param name="message">Message that need to sent to message broker</param>
        public Task Notify(string queueName, string message);
    }
}