using Fraud.Entities.DTOs;
using Fraud.Entities.Models;

namespace Fraud.UseCase.Transactions
{
    public interface ITransactionAnalyzer
    {
        TransactionAnalyzerResult AnalyzeTransactions(in Transaction[] transactions);
    }
}