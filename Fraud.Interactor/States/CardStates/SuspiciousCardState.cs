using System.Threading.Tasks;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.UseCase.Cards;

namespace Fraud.Interactor.States.CardStates
{
    public class SuspiciousCardState : ICardState
    {
        public Card Card { get; set; }
        public CardState CardState => CardState.Suspicious;
        
        public SuspiciousCardState(Card card)
        {
            Card = card;
        }
        
        public async Task HandleState()
        {
            throw new System.NotImplementedException();
        }
    }
}