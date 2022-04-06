using System.Threading.Tasks;
using Fraud.Entities.DTOs;

namespace Fraud.UseCase.Cards
{
    public interface ICardStateManagement
    {
        public Task ManageCardState(TransactionAnalyzerResult transactionAnalyzerResult);
    }
}