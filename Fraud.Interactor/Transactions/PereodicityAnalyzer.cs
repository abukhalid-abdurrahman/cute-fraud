using System;
using System.Collections.Generic;
using Fraud.Entities.DTOs;
using Fraud.Entities.Models;
using Fraud.UseCase.Transactions;

namespace Fraud.Interactor.Transactions
{
    public class PeriodicityAnalyzer : ITransactionAnalyzer
    {
        private readonly int _fraudPriorityStep = 30;
        
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
                if (transactionInterval <= 60)
                    fraudPriority += _fraudPriorityStep;
            }
            
            return new TransactionAnalyzerResult(fraudPriority, transactions[0].CardToken);
        }
    }
}