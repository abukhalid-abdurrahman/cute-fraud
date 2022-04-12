using System;
using System.Text;
using Fraud.Presentation.Configurations;
using Fraud.UseCase.MessageBroking;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Fraud.Interactor.MessageBroking
{
    public class RabbitMqMessageBrokerService : IMessageBrokerService
    {
        private readonly RabbitMqConfigurations _configurations;

        private IConnection _connection;
        private IModel _connectionModel;

        private bool _disposed;

        public RabbitMqMessageBrokerService(RabbitMqConfigurations configurations)
        {
            _configurations = configurations;

            InitializeRabbitMqConnections();
        }

        public void Receive(string queueName, Action<byte[]> receiveAction)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RabbitMqMessageBrokerService));

            if (receiveAction == null)
                throw new ArgumentNullException(nameof(receiveAction));

            _connectionModel.BasicQos(
                0,
                1,
                false);

            var consumer = new EventingBasicConsumer(_connectionModel);
            consumer.Received += (_, deliverArgs) => receiveAction(deliverArgs.Body.ToArray());

            _connectionModel.BasicConsume(
                queueName,
                true,
                consumer);
        }

        public void Send(string routingKey, string exchangeName, string message)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RabbitMqMessageBrokerService));

            Send(exchangeName, routingKey, Encoding.UTF8.GetBytes(message));
        }

        public void Send(string routingKey, string exchangeName, byte[] bufferMessage)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RabbitMqMessageBrokerService));

            var properties = _connectionModel.CreateBasicProperties();
            properties.Persistent = false;
            _connectionModel.BasicPublish(exchangeName, routingKey, properties, bufferMessage);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();

            // Do not call finalizer, if dispose method called
            GC.SuppressFinalize(this);
        }

        private void InitializeRabbitMqConnections()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = _configurations.HostName,
                Password = _configurations.Password,
                UserName = _configurations.UserName
            };
            _connection = connectionFactory.CreateConnection();
            _connectionModel = _connection.CreateModel();
        }

        private void ReleaseUnmanagedResources()
        {
            if(_disposed) return;
            
            _connectionModel.Close();
            _connectionModel.Dispose();

            _connection.Close();
            _connection.Dispose();

            _disposed = true;
        }

        ~RabbitMqMessageBrokerService()
        {
            ReleaseUnmanagedResources();
        }
    }
}