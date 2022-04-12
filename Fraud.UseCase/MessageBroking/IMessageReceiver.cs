using System;
using System.Threading.Tasks;

namespace Fraud.UseCase.MessageBroking
{
    public interface IMessageReceiver<out TActionArgument>
    {
        /// <summary>
        /// Listening specified queue, receives message and raise an event.
        /// </summary>
        /// <param name="queueName">Queue name for receiving messages</param>
        /// <param name="receiveAction">An action that will be invoked every time when message will be received</param>
        public Task InvokeActionOnMessageReceived(string queueName, Action<TActionArgument> receiveAction);
    }
}