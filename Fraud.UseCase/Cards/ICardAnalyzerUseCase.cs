using System.Threading.Tasks;

namespace Fraud.UseCase.Cards
{
    public interface ICardAnalyzerUseCase
    {
        public Task AnalyzeCard();
    }
}