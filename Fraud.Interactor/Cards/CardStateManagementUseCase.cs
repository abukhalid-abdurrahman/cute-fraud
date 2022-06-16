using System.Threading.Tasks;
using Fraud.Concerns.Configurations;
using Fraud.Entities.DTOs;
using Fraud.Infrastructure.Repository;
using Fraud.Interactor.States.CardStates;
using Fraud.UseCase.Cards;
using Fraud.UseCase.MessageBroking;

namespace Fraud.Interactor.Cards
{
    public class CardStateManagementUseCase : ICardStateManagementUseCase
    {
        private readonly ICardRepository _cardRepository;
        private readonly IMessageBrokerUseCase _messageBrokerUseCase;
        private readonly RabbitMqConfigurations _rabbitMqConfigurations;

        public CardStateManagementUseCase(ICardRepository cardRepository, IMessageBrokerUseCase messageBrokerUseCase,
            RabbitMqConfigurations rabbitMqConfigurations)
        {
            _cardRepository = cardRepository;
            _messageBrokerUseCase = messageBrokerUseCase;
            _rabbitMqConfigurations = rabbitMqConfigurations;
        }

        public async Task ManageCardState(TransactionAnalyzerResult transactionAnalyzerResult)
        {
            var card = await _cardRepository
                .UpdateCardPriority(transactionAnalyzerResult.CardToken, transactionAnalyzerResult.FraudPriority);
 
            ICardStateUseCase cardStateUseCase = card.FraudPriority switch
            {
                <= 0 => new DefaultCardStateUseCase(card),
                >= 35 and < 70 => new PreSuspiciousCardStateUseCase(card),
                >= 70 and < 80 => new SuspiciousCardStateUseCase(card, _messageBrokerUseCase, _rabbitMqConfigurations),
                >= 80 and < 90 => new TemporaryBlockedCardStateUseCase(card, _messageBrokerUseCase, _rabbitMqConfigurations),
                >= 90 => new BlockedCardStateUseCase(card, _messageBrokerUseCase, _rabbitMqConfigurations),
                _ => new DefaultCardStateUseCase(card)
            };

            await cardStateUseCase.HandleState();
        }
    }
}