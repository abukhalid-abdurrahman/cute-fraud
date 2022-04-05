using System.Threading.Tasks;
using Fraud.Entities.Models;

namespace Fraud.UseCase.Cards
{
    public interface ICardAnalyzerUseCase
    {
        public Task AnalyzeCard(Transaction transaction);
    }
}