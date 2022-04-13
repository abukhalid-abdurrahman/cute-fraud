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
    public class PeriodicityTransactionAnalyzerTests
    {
        private readonly float _highPriorityValue = 45;
        private readonly float _lowPriorityValue = 15;

        private Mock<ICardStateManagement> _cardStateManagement;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _cardStateManagement = new Mock<ICardStateManagement>();
            _cardStateManagement
                .Setup(x => x.ManageCardState(It.IsAny<TransactionAnalyzerResult>()));
        }
        
        [Test]
        public void PeriodicityAnalyzer_AnalyzeTransactions_ShouldReturnMinimalFraudPriority()
        {
            ITransactionAnalyzer transactionAnalyzer = new PeriodicityAnalyzer(_cardStateManagement.Object);
            var transactionAnalyzerResult = transactionAnalyzer.AnalyzeTransactions(new[]
            {
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddHours(-2).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddHours(-8).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddHours(-24).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddHours(-32).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddHours(-40).ToUnixTimeSeconds()
                },
            });
            Assert.LessOrEqual(transactionAnalyzerResult.FraudPriority, _lowPriorityValue);
        }
        
        
        [Test]
        public void PeriodicityAnalyzer_AnalyzeTransactions_ShouldReturnHighFraudPriority()
        {
            ITransactionAnalyzer transactionAnalyzer = new PeriodicityAnalyzer(_cardStateManagement.Object);
            var transactionAnalyzerResult = transactionAnalyzer.AnalyzeTransactions(new[]
            {
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-2).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-3).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-4).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-5).ToUnixTimeSeconds()
                },
                new Transaction()
                {
                    DateCreatedUnix = DateTimeOffset.Now.AddMinutes(-6).ToUnixTimeSeconds()
                },
            });
            Assert.GreaterOrEqual(transactionAnalyzerResult.FraudPriority, _highPriorityValue);
        }
    }
}