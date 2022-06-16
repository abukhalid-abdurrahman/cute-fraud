using System;
using System.Linq;
using System.Threading.Tasks;
using Fraud.Entities.DTOs;
using Fraud.Entities.Models;
using Fraud.UseCase.Cards;
using Fraud.UseCase.Transactions;

namespace Fraud.Interactor.Transactions
{
    /// <summary>
    /// Analyze transactions through average time interval
    /// </summary>
    public class CountAnalyzerUseCase : ITransactionAnalyzerUseCase
    {
        private readonly int _fraudPriorityStep = 10;
        
        private readonly ICardStateManagementUseCase _cardStateManagementUseCase;

        public CountAnalyzerUseCase(ICardStateManagementUseCase cardStateManagementUseCase)
        {
            _cardStateManagementUseCase = cardStateManagementUseCase;
        }

        public TransactionAnalyzerResult AnalyzeTransactions(in Transaction[] transactions)
        {
            if (transactions == null)
                throw new ArgumentNullException(nameof(transactions));

            var fraudPriority = 0;
            var iterationsCount = transactions.Length - 1;

            var transactionIntervals = new long[transactions.Length - 1];
            
            for (var i = 0; i < iterationsCount; i++)
            {
                var nextTransactionIndex = i + 1;
                if(nextTransactionIndex > iterationsCount)
                    break;
                
                var transactionInterval = 
                    Math.Abs(transactions[nextTransactionIndex].DateCreatedUnix - transactions[i].DateCreatedUnix);
                transactionIntervals[i] = transactionInterval;
            }

            var intervalAvg = transactionIntervals.Average();
            for (var i = 0; i < transactionIntervals.Length - 1; i++)
            {
                if (transactionIntervals[i] < intervalAvg)
                    fraudPriority += _fraudPriorityStep;
            }

            var transactionAnalyzerResult = new TransactionAnalyzerResult(fraudPriority, transactions[0].SenderCardToken);

            // TODO: Remove calling ICardStateManagement.ManageCardState after moving to microservices 
            Task.Run(() => _cardStateManagementUseCase.ManageCardState(transactionAnalyzerResult));

            return transactionAnalyzerResult;
        }
    }
}