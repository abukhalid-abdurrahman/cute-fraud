using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;

namespace Fraud.UseCase.Cards
{
    public interface ICardStateUseCase
    {
        public Card Card { get; set; }
        public CardState CardState { get; }
        public Task<ReturnResult<bool>> HandleState();
    }
}