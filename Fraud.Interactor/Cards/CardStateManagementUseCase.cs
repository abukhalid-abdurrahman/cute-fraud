using System.Threading.Tasks;
using Fraud.Concerns;
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

        public async Task<ReturnResult<bool>> ManageCardState(TransactionAnalyzerResult transactionAnalyzerResult)
        {
            var updateCardPriorityResult = await _cardRepository
                .UpdateCardPriority(transactionAnalyzerResult.CardToken, transactionAnalyzerResult.FraudPriority);
 
            ICardStateUseCase cardStateUseCase = updateCardPriorityResult.Result.FraudPriority switch
            {
                <= 0 => new DefaultCardStateUseCase(updateCardPriorityResult.Result),
                >= 35 and < 70 => new PreSuspiciousCardStateUseCase(updateCardPriorityResult.Result),
                >= 70 and < 80 => new SuspiciousCardStateUseCase(updateCardPriorityResult.Result, _messageBrokerUseCase, _rabbitMqConfigurations),
                >= 80 and < 90 => new TemporaryBlockedCardStateUseCase(updateCardPriorityResult.Result, _messageBrokerUseCase, _rabbitMqConfigurations),
                >= 90 => new BlockedCardStateUseCase(updateCardPriorityResult.Result, _messageBrokerUseCase, _rabbitMqConfigurations),
                _ => new DefaultCardStateUseCase(updateCardPriorityResult.Result)
            };
            await cardStateUseCase.HandleState();

            return ReturnResult<bool>.SuccessResult();
        }
    }
}