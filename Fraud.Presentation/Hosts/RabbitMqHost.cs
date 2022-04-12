using System.Threading;
using System.Threading.Tasks;
using Fraud.Presentation.Services.MessageHandler;
using Fraud.UseCase.MessageBroking;
using Microsoft.Extensions.Hosting;

namespace Fraud.Presentation.Hosts
{
    public class RabbitMqHost : IHostedService
    {
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly IMessageHandlerService _messageHandlerService;

        public RabbitMqHost(IMessageBrokerService messageBrokerService, IMessageHandlerService messageHandlerService)
        {
            _messageBrokerService = messageBrokerService;
            _messageHandlerService = messageHandlerService;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _messageBrokerService.Receive(
                queueName: "", 
                receiveAction: buffer => _messageHandlerService.HandleMessage(buffer));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageBrokerService.Dispose();
            return Task.CompletedTask;
        }
    }
}