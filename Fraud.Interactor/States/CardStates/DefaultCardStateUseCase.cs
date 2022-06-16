using System.Threading.Tasks;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.UseCase.Cards;

namespace Fraud.Interactor.States.CardStates
{
    public class DefaultCardStateUseCase : ICardStateUseCase
    {
        public Card Card { get; set; }
        public CardState CardState => CardState.Default;

        public DefaultCardStateUseCase(Card card)
        {
            Card = card;
        }

        public Task HandleState()
        {
            return Task.CompletedTask;
        }
    }
}