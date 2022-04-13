using System;
using Fraud.Entities.DTOs;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.Interactor.Transactions;
using Fraud.UseCase.Cards;
using Fraud.UseCase.Transactions;
using Moq;
using NUnit.Framework;

namespace Fraud.Tests.UseCase.Tests
{
    [TestFixture]
    public class AmountTransactionAnalyzerTests
    {
        private readonly float _highPriorityValue = 45;
        private readonly float _lowPriorityValue = 15;
        
        [Test]
        public void AmountAnalyzer_AnalyzeTransactions_ShouldReturnMinimalPriorityOfFraud()
        {
            var cardStateManagement = new Mock<ICardStateManagement>();
            cardStateManagement
                .Setup(x => x.ManageCardState(It.IsAny<TransactionAnalyzerResult>()));
            ITransactionAnalyzer transactionAnalyzer = new AmountAnalyzer(cardStateManagement.Object);
            var transactionAnalyzerResult = transactionAnalyzer.AnalyzeTransactions(new[]
            {
                new Transaction()
                {
                    Amount = 1100,
                    CardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddHours(-3).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    Amount = 1110,
                    CardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddHours(-5).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    Amount = 1100,
                    CardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddHours(-1).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    Amount = 1140,
                    CardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddDays(-1).ToUnixTimeSeconds()
                }
            });
            Assert.Less(transactionAnalyzerResult.FraudPriority, _lowPriorityValue);
        }
        
        [Test]
        public void AmountAnalyzer_AnalyzeTransactions_ShouldReturnHighPriorityOfFraud()
        {
            var cardStateManagement = new Mock<ICardStateManagement>();
            cardStateManagement
                .Setup(x => x.ManageCardState(It.IsAny<TransactionAnalyzerResult>()));
            ITransactionAnalyzer transactionAnalyzer = new AmountAnalyzer(cardStateManagement.Object);
            var transactionAnalyzerResult = transactionAnalyzer.AnalyzeTransactions(new[]
            {
                new Transaction()
                {
                    Amount = 3000,
                    CardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-1).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    Amount = 1200,
                    CardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-2).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    Amount = 1260,
                    CardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-3).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    Amount = 1610,
                    CardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-4).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    Amount = 550,
                    CardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-5).ToUnixTimeSeconds()
                }
            });
            Assert.GreaterOrEqual(transactionAnalyzerResult.FraudPriority, _highPriorityValue);
        }
    }
}