using System;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Concerns.FaultHandling;
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
        
        public async Task<ReturnResult<bool>> AnalyzeCard(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            var returnResult = new ReturnResult<bool>();
            var errorMessage = "Error was occured in CardAnalyzerInteractor.AnalyzeCard()! Reason: {0}";
            
            // Creates a new transaction in db
            var transactionCreationResult = await _transactionRepository.Create(transaction);
            if (!transactionCreationResult.IsSuccessfully)
            {
                FaultHandler.HandleError(ref returnResult, 
                    transactionCreationResult.Exception, 
                    string.Format(errorMessage, transactionCreationResult.Message), 
                    transactionCreationResult.DetailedMessage);
                return returnResult;
            }

            var dateRangedTransactionsResult = 
                await _transactionRepository.FindByDateRange(transaction.SenderCardToken, 
                    DateUtils.GetStartDate().AddDays(-5), DateUtils.GetEndOfTheDate());

            if (!dateRangedTransactionsResult.IsSuccessfully)
            {
                FaultHandler.HandleWarning(ref returnResult, 
                    dateRangedTransactionsResult.Message, 
                    string.Format(errorMessage, dateRangedTransactionsResult.DetailedMessage));
                return returnResult;
            }
            
            await Task.Run(() => _amountAnalyzerUseCase.AnalyzeTransactions(dateRangedTransactionsResult.Result));
            await Task.Run(() => _periodicityAnalyzerUseCase.AnalyzeTransactions(dateRangedTransactionsResult.Result));
            await Task.Run(() => _countAnalyzerUseCase.AnalyzeTransactions(dateRangedTransactionsResult.Result));
            
            return ReturnResult<bool>.SuccessResult(true);
        }
    }
}