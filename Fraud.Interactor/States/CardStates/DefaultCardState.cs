using System.Threading.Tasks;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.UseCase.Cards;

namespace Fraud.Interactor.States.CardStates
{
    public class DefaultCardState : ICardState
    {
        public Card Card { get; set; }
        public CardState CardState => CardState.Default;

        public DefaultCardState(Card card)
        {
            Card = card;
        }

        public Task HandleState()
        {
            return Task.CompletedTask;
        }
    }
}