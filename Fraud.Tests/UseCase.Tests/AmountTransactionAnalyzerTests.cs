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
        
        private Mock<ICardStateManagementUseCase> _cardStateManagement;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _cardStateManagement = new Mock<ICardStateManagementUseCase>();
            _cardStateManagement
                .Setup(x => x.ManageCardState(It.IsAny<TransactionAnalyzerResult>()));
        }
        
        [Test]
        public void AmountAnalyzer_AnalyzeTransactions_ShouldReturnMinimalPriorityOfFraud()
        {
            ITransactionAnalyzerUseCase transactionAnalyzerUseCase = new AmountAnalyzerUseCase(_cardStateManagement.Object);
            var transactionAnalyzerResult = transactionAnalyzerUseCase.AnalyzeTransactions(new[]
            {
                new Transaction()
                {
                    Amount = 1100,
                    SenderCardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddHours(-3).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    Amount = 1110,
                    SenderCardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddHours(-5).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    Amount = 1100,
                    SenderCardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddHours(-1).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    Amount = 1140,
                    SenderCardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddDays(-1).ToUnixTimeSeconds()
                }
            });
            Assert.Less(transactionAnalyzerResult.Result.FraudPriority, _lowPriorityValue);
        }
        
        [Test]
        public void AmountAnalyzer_AnalyzeTransactions_ShouldReturnHighPriorityOfFraud()
        {
            ITransactionAnalyzerUseCase transactionAnalyzerUseCase = new AmountAnalyzerUseCase(_cardStateManagement.Object);
            var transactionAnalyzerResult = transactionAnalyzerUseCase.AnalyzeTransactions(new[]
            {
                new Transaction()
                {
                    Amount = 3000,
                    SenderCardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-1).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    Amount = 1200,
                    SenderCardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-2).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    Amount = 1260,
                    SenderCardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-3).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    Amount = 1610,
                    SenderCardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-4).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    Amount = 550,
                    SenderCardToken = Guid.NewGuid().ToString(),
                    ExternalRef = Guid.NewGuid().ToString(),
                    TransactionState = TransactionState.Default,
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-5).ToUnixTimeSeconds()
                }
            });
            Assert.GreaterOrEqual(transactionAnalyzerResult.Result.FraudPriority, _highPriorityValue);
        }
    }
}