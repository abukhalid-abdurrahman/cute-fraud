using System.Threading.Tasks;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;

namespace Fraud.UseCase.Cards
{
    public interface ICardState
    {
        public Card Card { get; set; }
        public CardState CardState { get; }
        public Task HandleState();
    }
}