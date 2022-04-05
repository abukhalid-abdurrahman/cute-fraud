using System;
using System.Threading.Tasks;
using Fraud.Entities.Models;
using Fraud.Infrastructure.Repository;
using Fraud.UseCase.Cards;

namespace Fraud.Interactor.CardAnalyzer
{
    public class CardAnalyzerInteractor : ICardAnalyzerUseCase
    {
        private readonly ITransactionRepository _transactionRepository;

        public CardAnalyzerInteractor(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        
        public async Task AnalyzeCard(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));
            
            // Creates a new transaction in db
            await _transactionRepository.Create(transaction);
        }
    }
}