using System;
using System.Linq;
using System.Threading.Tasks;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.Infrastructure.Implementation.Configurations;
using Fraud.Infrastructure.Repository;
using Neo4j.Driver;

namespace Fraud.Infrastructure.Implementation.Repository
{
    public class Neo4JCardRepository : ICardRepository
    {
        private readonly IDriver _driver;
        private bool _disposed = false;
        
        public Neo4JCardRepository(Neo4JConfigurations neo4JConfigurations)
        {
            _driver = GraphDatabase.Driver(neo4JConfigurations.Uri, 
                AuthTokens.Basic(neo4JConfigurations.UserName, neo4JConfigurations.Password));
        }
        
        public async Task<Card> UpdateCardPriority(string cardToken, float fraudPriority)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Neo4JTransactionRepository));
            
            const string query = @"
                MERGE (c:card { card_token: $CardToken, fraud_priority: $FraudPriority })
                SET c.fraud_priority = $FraudPriority
                RETURN c;";

            var session = _driver.AsyncSession();
            try
            {
                var writeResults = await session.WriteTransactionAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new
                    {
                        CardToken = cardToken,
                        FraudPriority = fraudPriority
                    });
                    return await result.ToListAsync();
                });
                return writeResults.Select(x => new Card
                {
                    CardToken = x["c.card_token"].As<string>(),
                    FraudPriority = x["c.card_token"].As<double>(),
                    CardState = CardState.Default
                }).FirstOrDefault();
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

        ~Neo4JCardRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}