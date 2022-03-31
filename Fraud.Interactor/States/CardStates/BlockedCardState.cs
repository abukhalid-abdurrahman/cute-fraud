using System.Threading.Tasks;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.UseCase.States;

namespace Fraud.Interactor.States.CardStates
{
    public class BlockedCardState : ICardState
    {
        public Card Card { get; set; }
        public CardState CardState => CardState.Blocked;
        
        public async Task HandleState()
        {
            throw new System.NotImplementedException();
        }
    }
}