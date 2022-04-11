using System.Threading.Tasks;
using Fraud.Entities.DTOs;
using Fraud.UseCase.Cards;

namespace Fraud.Interactor.Cards
{
    public class CardStateManagement : ICardStateManagement
    {
        public CardStateManagement()
        {
            
        }
        
        public async Task ManageCardState(TransactionAnalyzerResult transactionAnalyzerResult)
        {
            throw new System.NotImplementedException();
        }
    }
}