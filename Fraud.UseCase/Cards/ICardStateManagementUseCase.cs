using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.DTOs;

namespace Fraud.UseCase.Cards
{
    public interface ICardStateManagementUseCase
    {
        public Task<ReturnResult<bool>> ManageCardState(TransactionAnalyzerResult transactionAnalyzerResult);
    }
}