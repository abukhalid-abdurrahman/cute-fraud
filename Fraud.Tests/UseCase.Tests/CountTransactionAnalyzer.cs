using System;
using Fraud.Entities.DTOs;
using Fraud.Entities.Models;
using Fraud.Interactor.Transactions;
using Fraud.UseCase.Cards;
using Fraud.UseCase.Transactions;
using Moq;
using NUnit.Framework;

namespace Fraud.Tests.UseCase.Tests
{
    [TestFixture]
    public class CountTransactionAnalyzer
    {
        private readonly float _highPriorityValue = 40;
        private readonly float _lowPriorityValue = 10;

        private Mock<ICardStateManagement> _cardStateManagement;
        
        [OneTimeSetUp]
        public void SetUp()
        {
            _cardStateManagement = new Mock<ICardStateManagement>();
            _cardStateManagement
                .Setup(x => x.ManageCardState(It.IsAny<TransactionAnalyzerResult>()));
        }

        [Test]
        public void CountAnalyzer_AnalyzeTransactions_ShouldReturnMinimalPriorityOfFraud()
        {
            ITransactionAnalyzer transactionAnalyzer = new CountAnalyzer(_cardStateManagement.Object);
            var transactionAnalyzerResult = transactionAnalyzer.AnalyzeTransactions(new []
            {
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMonths(-1).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMonths(-2).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMonths(-3).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMonths(-4).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMonths(-5).ToUnixTimeSeconds()
                }
            });
            Assert.LessOrEqual(transactionAnalyzerResult.FraudPriority, _lowPriorityValue);
        }

        [Test]
        public void CountAnalyzer_AnalyzeTransactions_ShouldReturnHighFraudPriority()
        {
            ITransactionAnalyzer transactionAnalyzer = new CountAnalyzer(_cardStateManagement.Object);
            var transactionAnalyzerResult = transactionAnalyzer.AnalyzeTransactions(new []
            {
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddDays(-1).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddDays(-1).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddDays(-1).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddDays(-1).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddDays(-1).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMonths(-1).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMonths(-2).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMonths(-3).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMonths(-4).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMonths(-5).ToUnixTimeSeconds()
                }
            });
            Assert.GreaterOrEqual(transactionAnalyzerResult.FraudPriority, _highPriorityValue);
        }
    }
}