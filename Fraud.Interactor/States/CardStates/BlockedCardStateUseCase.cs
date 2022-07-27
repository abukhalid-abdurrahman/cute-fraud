using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Concerns.Configurations;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.UseCase.Cards;
using Fraud.UseCase.MessageBroking;
using Newtonsoft.Json;

namespace Fraud.Interactor.States.CardStates
{
    public class BlockedCardStateUseCase : ICardStateUseCase
    {
        private readonly IMessageBrokerUseCase _messageBrokerUseCase;
        private readonly RabbitMqConfigurations _rabbitMqConfigurations;

        public BlockedCardStateUseCase(Card card, IMessageBrokerUseCase messageBrokerUseCase,
            RabbitMqConfigurations rabbitMqConfigurations)
        {
            _messageBrokerUseCase = messageBrokerUseCase;
            _rabbitMqConfigurations = rabbitMqConfigurations;
            Card = card;
        }

        public Card Card { get; set; }
        public CardState CardState => CardState.Blocked;

        public async Task<ReturnResult<bool>> HandleState()
        {
            var cardStateMessage = JsonConvert.SerializeObject(Card);
            _messageBrokerUseCase.Send(_rabbitMqConfigurations.BlockCardRoutingKey,
                _rabbitMqConfigurations.BlockCardExchangeName,
                cardStateMessage);
            
            return ReturnResult<bool>.SuccessResult();
        }
    }
}