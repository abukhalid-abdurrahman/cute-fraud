using System;
using System.Threading.Tasks;
using Fraud.Entities.DTOs;
using Fraud.Entities.Models;
using Fraud.UseCase.Cards;
using Fraud.UseCase.Transactions;

namespace Fraud.Interactor.Transactions
{
    /// <summary>
    /// Analyze transactions through specified time interval
    /// </summary>
    public class PeriodicityAnalyzer : ITransactionAnalyzer
    {
        private readonly int _fraudPriorityStep = 30;
        private readonly int _transactionsIntervalInSeconds = 60;

        private readonly ICardStateManagement _cardStateManagement;

        public PeriodicityAnalyzer(ICardStateManagement cardStateManagement)
        {
            _cardStateManagement = cardStateManagement;
        }
        
        public TransactionAnalyzerResult AnalyzeTransactions(in Transaction[] transactions)
        {            
            if (transactions == null)
                throw new ArgumentNullException(nameof(transactions));

            var fraudPriority = 0;
            var iterationsCount = transactions.Length - 1;

            for (var i = 0; i < iterationsCount; i++)
            {
                var nextTransactionIndex = i + 1;
                if(nextTransactionIndex > iterationsCount)
                    break;
                
                var transactionInterval = 
                    Math.Abs(transactions[nextTransactionIndex].DateCreatedUnix - transactions[i].DateCreatedUnix);
                if (transactionInterval <= _transactionsIntervalInSeconds)
                    fraudPriority += _fraudPriorityStep;
            }

            var transactionAnalyzerResult = new TransactionAnalyzerResult(fraudPriority, transactions[0].SenderCardToken);

            // TODO: Remove calling ICardStateManagement.ManageCardState after moving to microservices 
            Task.Run(() => _cardStateManagement.ManageCardState(transactionAnalyzerResult));

            return transactionAnalyzerResult;
        }
    }
}