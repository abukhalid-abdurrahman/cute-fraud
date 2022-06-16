using Fraud.Concerns;
using Fraud.Entities.DTOs;
using Fraud.Entities.Models;

namespace Fraud.UseCase.Transactions
{
    public interface ITransactionAnalyzerUseCase
    {
        ReturnResult<TransactionAnalyzerResult> AnalyzeTransactions(in Transaction[] transactions);
    }
}