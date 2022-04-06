using System;
using System.Linq;
using Fraud.Entities.DTOs;
using Fraud.Entities.Models;
using Fraud.UseCase.Transactions;

namespace Fraud.Interactor.Transactions
{
    public class CountAnalyzer : ITransactionAnalyzer
    {
        private readonly int _fraudPriorityStep = 30;

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

            return new TransactionAnalyzerResult(fraudPriority, transactions[0].CardToken);
        }
    }
}