using System.Threading.Tasks;
using Fraud.Concerns.Configurations;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.Interactor.MessageBroking;
using Fraud.UseCase.Cards;
using Fraud.UseCase.MessageBroking;
using Newtonsoft.Json;

namespace Fraud.Interactor.States.CardStates
{
    public class TemporaryBlockedCardState : ICardState
    {
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly RabbitMqConfigurations _rabbitMqConfigurations;
        
        public Card Card { get; set; }
        public CardState CardState => CardState.TemporaryBlocked;
        
        public TemporaryBlockedCardState(Card card, IMessageBrokerService messageBrokerService,
            RabbitMqConfigurations rabbitMqConfigurations)
        {
            _messageBrokerService = messageBrokerService;
            _rabbitMqConfigurations = rabbitMqConfigurations;
            Card = card;
        }
        
        public async Task HandleState()
        {
            var cardStateMessage = JsonConvert.SerializeObject(this.Card);
            _messageBrokerService.Send(_rabbitMqConfigurations.TemporaryBlockCardRoutingKey,
                _rabbitMqConfigurations.TemporaryBlockCardExchangeName,
                cardStateMessage);     
        }
    }
}