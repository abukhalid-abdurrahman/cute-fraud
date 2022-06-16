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
        private readonly ITransactionAnalyzerUseCase _amountAnalyzerUseCase;
        private readonly ITransactionAnalyzerUseCase _periodicityAnalyzerUseCase;
        private readonly ITransactionAnalyzerUseCase _countAnalyzerUseCase;
        
        public CardAnalyzerInteractor(ITransactionRepository transactionRepository, 
            AmountAnalyzerUseCase amountAnalyzerUseCase, PeriodicityAnalyzerUseCase periodicityAnalyzerUseCase, CountAnalyzerUseCase countAnalyzerUseCase)
        {
            _transactionRepository = transactionRepository;

            _amountAnalyzerUseCase = amountAnalyzerUseCase;
            _periodicityAnalyzerUseCase = periodicityAnalyzerUseCase;
            _countAnalyzerUseCase = countAnalyzerUseCase;
        }
        
        public async Task AnalyzeCard(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));
            
            // Creates a new transaction in db
            await _transactionRepository.Create(transaction);
            
            var dateRangedTransactions = 
                await _transactionRepository.FindByDateRange(transaction.SenderCardToken, 
                    DateUtils.GetStartDate().AddDays(-5), DateUtils.GetEndOfTheDate());
            
            await Task.Run(() => _amountAnalyzerUseCase.AnalyzeTransactions(dateRangedTransactions));
            await Task.Run(() => _periodicityAnalyzerUseCase.AnalyzeTransactions(dateRangedTransactions));
            await Task.Run(() => _countAnalyzerUseCase.AnalyzeTransactions(dateRangedTransactions));
        }
    }
}