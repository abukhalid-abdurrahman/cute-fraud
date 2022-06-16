using System;
using System.Threading;
using System.Threading.Tasks;
using Fraud.Concerns.Configurations;
using Fraud.Presentation.Services.MessageHandler;
using Fraud.UseCase.MessageBroking;
using Microsoft.Extensions.Hosting;

namespace Fraud.Presentation.Hosts
{
    public class RabbitMqHost : IHostedService, IDisposable
    {
        private readonly IMessageBrokerUseCase _messageBrokerUseCase;
        private readonly IMessageHandlerService _messageHandlerService;
        private readonly RabbitMqConfigurations _rabbitMqConfigurations;

        private Timer _timer;

        public RabbitMqHost(IMessageBrokerUseCase messageBrokerUseCase, IMessageHandlerService messageHandlerService, RabbitMqConfigurations rabbitMqConfigurations)
        {
            _messageBrokerUseCase = messageBrokerUseCase;
            _messageHandlerService = messageHandlerService;
            _rabbitMqConfigurations = rabbitMqConfigurations;
        }
        
        private void DoWork(object state)
        {
            _messageBrokerUseCase.Receive(
                    queueName: _rabbitMqConfigurations.P2PQueueName, 
                    receiveAction: buffer => _messageHandlerService.HandleMessage(buffer));
        }  
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Timer will fire 10 seconds after start
            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(10).Seconds, Timeout.Infinite);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);       
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _messageBrokerUseCase.Dispose();
            _timer?.Dispose();
        }
    }
}