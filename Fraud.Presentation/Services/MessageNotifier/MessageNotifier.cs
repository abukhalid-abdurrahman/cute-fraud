using System.Threading.Tasks;
using Fraud.Presentation.Enums;
using Fraud.Presentation.Services.MessageBroker;

namespace Fraud.Presentation.Services.MessageNotifier
{
    public class MessageNotifier : IMessageNotifier
    {
        private readonly IMessageBrokerService _messageBrokerService;

        public MessageNotifier(IMessageBrokerService messageBrokerService)
        {
            _messageBrokerService = messageBrokerService;
        }
        
        public async Task Notify<TMessageObject>(TMessageObject messageObject, MessageType messageType)
        {
        }
    }
}