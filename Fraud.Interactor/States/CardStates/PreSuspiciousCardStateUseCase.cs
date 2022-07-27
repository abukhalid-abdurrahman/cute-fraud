using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.UseCase.Cards;

namespace Fraud.Interactor.States.CardStates
{
    public class PreSuspiciousCardStateUseCase : ICardStateUseCase
    {
        public Card Card { get; set; }
        public CardState CardState => CardState.PreSuspicious;
        
        public PreSuspiciousCardStateUseCase(Card card)
        {
            Card = card;
        }
        
        public async Task<ReturnResult<bool>> HandleState()
        {
            return ReturnResult<bool>.SuccessResult();
        }
    }
}