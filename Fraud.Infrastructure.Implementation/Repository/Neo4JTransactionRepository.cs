using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.Infrastructure.Implementation.Configurations;
using Fraud.Infrastructure.Repository;
using Neo4j.Driver;

namespace Fraud.Infrastructure.Implementation.Repository
{
    public class Neo4JTransactionRepository : ITransactionRepository
    {
        private readonly IDriver _driver;
        private bool _disposed = false;

        public Neo4JTransactionRepository(Neo4JConfigurations neo4JConfigurations)
        {
            _driver = GraphDatabase.Driver(neo4JConfigurations.Uri, 
                AuthTokens.Basic(neo4JConfigurations.UserName, neo4JConfigurations.Password));
        }

        public async Task Create(Transaction entity)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Neo4JTransactionRepository));
            const string query = @"
                MERGE (t:transaction { amount: $Amount, external_ref: $ExternalRef, card_token: $CardToken, transaction_state: $TransactionState, date_created_unix: $DateCreatedUnix })
                RETURN t;";

            var session = _driver.AsyncSession();
            try
            {
                await session.WriteTransactionAsync(async tx =>
                {
                    await tx.RunAsync(query, new
                    {
                        entity.Amount,
                        entity.CardToken,
                        entity.ExternalRef,
                        TransactionState = (int) entity.TransactionState,
                        entity.DateCreatedUnix
                    });
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<IReadOnlyList<Transaction>> FindByDateRange(string cardToken, DateTimeOffset dateFrom, DateTimeOffset dateTo)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Neo4JTransactionRepository));
            
            const string query = @"
                MATCH (t:transaction)
                WHERE t.date_created_unix >= $DateFromUnix AND t.date_created_unix <= $DateToUnix AND t.card_token = $CardToken
                RETURN t";

            var session = _driver.AsyncSession();
            try
            {
                var readResults = await session.ReadTransactionAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, 
                        new { DateFromUnix = dateFrom.ToUnixTimeSeconds(), DateToUnix = dateTo.ToUnixTimeSeconds(), CardToken = cardToken });
                    return await result.ToListAsync();
                });
                return readResults.Select(x => new Transaction
                {
                    Amount = x["t.amount"].As<int>(),
                    CardToken = x["t.card_token"].As<string>(),
                    DateCreatedUnix = x["t.date_created_unix"].As<long>(),
                    ExternalRef = x["t.external_ref"].As<string>(),
                    TransactionState = (TransactionState)x["t.transaction_state"].As<int>()
                }).ToList();
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<IReadOnlyList<Transaction>> FindLimit(string cardToken, int limit = 100)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Neo4JTransactionRepository));
            
            const string query = @"
                MATCH (t:transaction)
                WHERE t.card_token = $CardToken
                RETURN t
                LIMIT $Limit";

            var session = _driver.AsyncSession();
            try
            {
                var readResults = await session.ReadTransactionAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new { Limit = limit, CardToken = cardToken });
                    return await result.ToListAsync();
                });
                return readResults.Select(x => new Transaction
                {
                    Amount = x["t.amount"].As<int>(),
                    CardToken = x["t.card_token"].As<string>(),
                    DateCreatedUnix = x["t.date_created_unix"].As<long>(),
                    ExternalRef = x["t.external_ref"].As<string>(),
                    TransactionState = (TransactionState)x["t.transaction_state"].As<int>()
                }).ToList();
            }
            finally
            {
                await session.CloseAsync();
            }        
        }

        private void ReleaseUnmanagedResources()
        {
            if (_disposed)
                return;
            
            _driver?.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~Neo4JTransactionRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}