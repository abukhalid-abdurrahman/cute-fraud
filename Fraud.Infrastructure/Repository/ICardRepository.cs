using System;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Models;

namespace Fraud.Infrastructure.Repository
{
    public interface ICardRepository : IDisposable
    {
        public Task<ReturnResult<Card>> UpdateCardPriority(string cardToken, float fraudPriority);
    }
}