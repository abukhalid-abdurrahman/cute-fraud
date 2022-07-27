using System;
using System.Linq;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Entities.DTOs;
using Fraud.Entities.Models;
using Fraud.UseCase.Cards;
using Fraud.UseCase.Transactions;

namespace Fraud.Interactor.Transactions
{
    public class AmountAnalyzerUseCase : ITransactionAnalyzerUseCase
    {
        private readonly int _fraudPriorityStep = 15;
        private readonly int _intervalInSeconds = 12 * 3600; // Six hours
        private readonly int _analyzeIterationsCount = 3;
        
        private readonly ICardStateManagementUseCase _cardStateManagementUseCase;

        public AmountAnalyzerUseCase(ICardStateManagementUseCase cardStateManagementUseCase)
        {
            _cardStateManagementUseCase = cardStateManagementUseCase;
        }

        public ReturnResult<TransactionAnalyzerResult> AnalyzeTransactions(in Transaction[] transactions)
        {
            if (transactions == null)
                throw new ArgumentNullException(nameof(transactions));

            var fraudPriority = 0;

            var transactionAmountAvg = transactions
                .OrderByDescending(x => x.DateCreatedUnix)
                .Skip(_analyzeIterationsCount)
                .Average(x => x.Amount);
            var transactionsByAmount = transactions
                .Where(x => (DateTimeOffset.Now.ToUnixTimeSeconds() - x.DateCreatedUnix) < _intervalInSeconds)
                .OrderByDescending(x => x.DateCreatedUnix)
                .ThenByDescending(x => x.Amount)
                .ToArray();
            
            for (var i = 0; i < _analyzeIterationsCount; i++)
            {
                if (transactionsByAmount[i]?.Amount > transactionAmountAvg)
                    fraudPriority += _fraudPriorityStep;
            }
            
            var transactionAnalyzerResult = new TransactionAnalyzerResult(fraudPriority, transactions[0].SenderCardToken);

            // TODO: Remove calling ICardStateManagement.ManageCardState after moving to microservices 
            Task.Run(() => _cardStateManagementUseCase.ManageCardState(transactionAnalyzerResult));

            return ReturnResult<TransactionAnalyzerResult>.SuccessResult(transactionAnalyzerResult);
        }
    }
}