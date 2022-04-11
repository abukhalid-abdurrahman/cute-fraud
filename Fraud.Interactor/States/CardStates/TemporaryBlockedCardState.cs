using System.Threading.Tasks;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.UseCase.Cards;

namespace Fraud.Interactor.States.CardStates
{
    public class TemporaryBlockedCardState : ICardState
    {
        public Card Card { get; set; }
        public CardState CardState => CardState.TemporaryBlocked;
        
        public TemporaryBlockedCardState(Card card)
        {
            Card = card;
        }
        
        public async Task HandleState()
        {
            throw new System.NotImplementedException();
        }
    }
}