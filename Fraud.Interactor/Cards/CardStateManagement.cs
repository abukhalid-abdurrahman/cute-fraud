using System.Threading.Tasks;
using Fraud.Entities.DTOs;
using Fraud.Entities.Models;
using Fraud.Infrastructure.Repository;
using Fraud.Interactor.States.CardStates;
using Fraud.UseCase.Cards;

namespace Fraud.Interactor.Cards
{
    public class CardStateManagement : ICardStateManagement
    {
        private readonly ICardRepository _cardRepository;

        public CardStateManagement(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public async Task ManageCardState(TransactionAnalyzerResult transactionAnalyzerResult)
        {
            var card = await _cardRepository
                .UpdateCardPriority(transactionAnalyzerResult.CardToken, transactionAnalyzerResult.FraudPriority);

            ICardState cardState = card.FraudPriority switch
            {
                <= 0 => new DefaultCardState(card),
                >= 35 and < 70 => new PreSuspiciousCardState(card),
                >= 70 and < 80 => new SuspiciousCardState(card),
                >= 80 and < 90 => new TemporaryBlockedCardState(card),
                >= 90 => new BlockedCardState(card),
                _ => new DefaultCardState(card)
            };

            await cardState.HandleState();
        }
    }
}