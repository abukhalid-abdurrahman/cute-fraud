using System;

namespace Fraud.UseCase.MessageBroking
{
    public interface IMessageBrokerUseCase : IDisposable
    {
        public void Send(string routingKey, string exchangeName, byte[] bufferMessage);
        public void Send(string routingKey, string exchangeName, string message);
        public void Receive(string queueName, Action<byte[]> receiveAction);
    }
}