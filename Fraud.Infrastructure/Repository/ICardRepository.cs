using System.Threading.Tasks;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface ICardRepository
    {
        public Task<Card> UpdateCardPriority(string cardToken, float fraudPriority);
    }
}