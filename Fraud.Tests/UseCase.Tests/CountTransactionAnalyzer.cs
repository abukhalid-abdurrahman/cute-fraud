﻿using System;
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

        private Mock<ICardStateManagementUseCase> _cardStateManagement;
        
        [OneTimeSetUp]
        public void SetUp()
        {
            _cardStateManagement = new Mock<ICardStateManagementUseCase>();
            _cardStateManagement
                .Setup(x => x.ManageCardState(It.IsAny<TransactionAnalyzerResult>()));
        }

        [Test]
        public void CountAnalyzer_AnalyzeTransactions_ShouldReturnMinimalPriorityOfFraud()
        {
            ITransactionAnalyzerUseCase transactionAnalyzerUseCase = new CountAnalyzerUseCase(_cardStateManagement.Object);
            var transactionAnalyzerResult = transactionAnalyzerUseCase.AnalyzeTransactions(new []
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
            Assert.LessOrEqual(transactionAnalyzerResult.Result.FraudPriority, _lowPriorityValue);
        }

        [Test]
        public void CountAnalyzer_AnalyzeTransactions_ShouldReturnHighFraudPriority()
        {
            ITransactionAnalyzerUseCase transactionAnalyzerUseCase = new CountAnalyzerUseCase(_cardStateManagement.Object);
            var transactionAnalyzerResult = transactionAnalyzerUseCase.AnalyzeTransactions(new []
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
            Assert.GreaterOrEqual(transactionAnalyzerResult.Result.FraudPriority, _highPriorityValue);
        }
    }
}