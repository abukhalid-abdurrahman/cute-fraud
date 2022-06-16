using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Models;

namespace Fraud.UseCase.Cards
{
    public interface ICardAnalyzerUseCase
    {
        public Task<ReturnResult<bool>> AnalyzeCard(Transaction transaction);
    }
}