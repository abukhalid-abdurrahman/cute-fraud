using System;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.Models;
using Fraud.Infrastructure.Repository;
using Fraud.Interactor.Transactions;
using Fraud.UseCase.Cards;
using Fraud.UseCase.Transactions;

namespace Fraud.Interactor.Cards
{
    public class CardAnalyzerInteractor : ICardAnalyzerUseCase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionAnalyzer _amountAnalyzer;
        private readonly ITransactionAnalyzer _periodicityAnalyzer;
        private readonly ITransactionAnalyzer _countAnalyzer;
        
        public CardAnalyzerInteractor(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;

            _amountAnalyzer = new AmountAnalyzer();
            _periodicityAnalyzer = new PeriodicityAnalyzer();
            _countAnalyzer = new CountAnalyzer();
        }
        
        public async Task AnalyzeCard(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));
            
            // Creates a new transaction in db
            await _transactionRepository.Create(transaction);
            
            var dateRangedTransactions = 
                await _transactionRepository.FindByDateRange(transaction.CardToken, 
                    DateUtils.GetStartDate().AddDays(-5), DateUtils.GetEndOfTheDate());
            
            await Task.Run(() => _amountAnalyzer.AnalyzeTransactions(dateRangedTransactions));
            await Task.Run(() => _periodicityAnalyzer.AnalyzeTransactions(dateRangedTransactions));
            await Task.Run(() => _countAnalyzer.AnalyzeTransactions(dateRangedTransactions));
        }
    }
}