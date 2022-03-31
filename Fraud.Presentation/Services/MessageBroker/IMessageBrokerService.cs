using System;

namespace Fraud.Presentation.Services.MessageBroker
{
    public interface IMessageBrokerService : IDisposable
    {
        public void Send(string routingKey, string exchangeName, byte[] bufferMessage);
        public void Send(string routingKey, string exchangeName, string message);
        public void Receive(string queueName, Action<byte[]> receiveAction);
    }
}