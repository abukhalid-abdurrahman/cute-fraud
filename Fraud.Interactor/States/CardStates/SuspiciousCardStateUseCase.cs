using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Concerns.Configurations;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.Interactor.MessageBroking;
using Fraud.UseCase.Cards;
using Fraud.UseCase.MessageBroking;
using Newtonsoft.Json;

namespace Fraud.Interactor.States.CardStates
{
    public class SuspiciousCardStateUseCase : ICardStateUseCase
    {
        private readonly IMessageBrokerUseCase _messageBrokerUseCase;
        private readonly RabbitMqConfigurations _rabbitMqConfigurations;
        
        public Card Card { get; set; }
        public CardState CardState => CardState.Suspicious;
        
        public SuspiciousCardStateUseCase(Card card, IMessageBrokerUseCase messageBrokerUseCase,
            RabbitMqConfigurations rabbitMqConfigurations)
        {
            _messageBrokerUseCase = messageBrokerUseCase;
            _rabbitMqConfigurations = rabbitMqConfigurations;
            Card = card;
        }
        
        public async Task<ReturnResult<bool>> HandleState()
        {
            var cardStateMessage = JsonConvert.SerializeObject(this.Card);
            _messageBrokerUseCase.Send(_rabbitMqConfigurations.SuspensionsCardRoutingKey,
                _rabbitMqConfigurations.SuspensionsCardExchangeName,
                cardStateMessage);
            return ReturnResult<bool>.SuccessResult();
        }
    }
}