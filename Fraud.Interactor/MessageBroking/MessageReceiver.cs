using System;
using System.Threading.Tasks;
using Fraud.UseCase.MessageBroking;

namespace Fraud.Interactor.MessageBroking
{
    public class MessageReceiver<TActionArgument> : IMessageReceiver<TActionArgument>
    {
        /// <summary>
        /// Listening specified queue, receives message and raise an event.
        /// </summary>
        /// <param name="queueName">Queue name for receiving messages</param>
        /// <param name="receiveAction">An action that will be invoked every time when message will be received</param>
        public async Task InvokeActionOnMessageReceived(string queueName, Action<TActionArgument> receiveAction)
        {
            
        }
    }
}