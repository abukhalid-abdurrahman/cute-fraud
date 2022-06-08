using System.Threading.Tasks;
using Fraud.Concerns.Configurations;
using Fraud.Entities.DTOs;
using Fraud.Infrastructure.Repository;
using Fraud.Interactor.States.CardStates;
using Fraud.UseCase.Cards;
using Fraud.UseCase.MessageBroking;

namespace Fraud.Interactor.Cards
{
    public class CardStateManagement : ICardStateManagement
    {
        private readonly ICardRepository _cardRepository;
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly RabbitMqConfigurations _rabbitMqConfigurations;

        public CardStateManagement(ICardRepository cardRepository, IMessageBrokerService messageBrokerService,
            RabbitMqConfigurations rabbitMqConfigurations)
        {
            _cardRepository = cardRepository;
            _messageBrokerService = messageBrokerService;
            _rabbitMqConfigurations = rabbitMqConfigurations;
        }

        public async Task ManageCardState(TransactionAnalyzerResult transactionAnalyzerResult)
        {
            var card = await _cardRepository
                .UpdateCardPriority(transactionAnalyzerResult.CardToken, transactionAnalyzerResult.FraudPriority);
 
            ICardState cardState = card.FraudPriority switch
            {
                <= 0 => new DefaultCardState(card),
                >= 35 and < 70 => new PreSuspiciousCardState(card),
                >= 70 and < 80 => new SuspiciousCardState(card, _messageBrokerService, _rabbitMqConfigurations),
                >= 80 and < 90 => new TemporaryBlockedCardState(card, _messageBrokerService, _rabbitMqConfigurations),
                >= 90 => new BlockedCardState(card, _messageBrokerService, _rabbitMqConfigurations),
                _ => new DefaultCardState(card)
            };

            await cardState.HandleState();
        }
    }
}