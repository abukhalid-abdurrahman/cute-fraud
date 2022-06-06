using System.Threading.Tasks;
using Fraud.Concerns.Configurations;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.UseCase.Cards;
using Fraud.UseCase.MessageBroking;
using Newtonsoft.Json;

namespace Fraud.Interactor.States.CardStates
{
    public class BlockedCardState : ICardState
    {
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly RabbitMqConfigurations _rabbitMqConfigurations;

        public BlockedCardState(Card card, IMessageBrokerService messageBrokerService,
            RabbitMqConfigurations rabbitMqConfigurations)
        {
            _messageBrokerService = messageBrokerService;
            _rabbitMqConfigurations = rabbitMqConfigurations;
            Card = card;
        }

        public Card Card { get; set; }
        public CardState CardState => CardState.Blocked;

        public async Task HandleState()
        {
            var cardStateMessage = JsonConvert.SerializeObject(Card);
            _messageBrokerService.Send(_rabbitMqConfigurations.BlockCardRoutingKey,
                _rabbitMqConfigurations.BlockCardExchangeName,
                cardStateMessage);
        }
    }
}