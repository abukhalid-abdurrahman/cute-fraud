using System;
using System.Linq;
using System.Threading.Tasks;
using Fraud.Concerns;
using Fraud.Concerns.Configurations;
using Fraud.Concerns.FaultHandling;
using Fraud.Entities.Enums;
using Fraud.Entities.Models;
using Fraud.Infrastructure.Repository;
using Neo4j.Driver;

namespace Fraud.Infrastructure.Implementation.Neo4JRepository
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
        
        public async Task<ReturnResult<Card>> UpdateCardPriority(string cardToken, float fraudPriority)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Neo4JTransactionRepository));
            
            const string query = @"
                MERGE (c:card { card_token: $CardToken, fraud_priority: $FraudPriority })
                SET c.fraud_priority = $FraudPriority
                RETURN c;";

            var session = _driver.AsyncSession();
            var returnResult = new ReturnResult<Card>();
            var errorMessageTemplate = "Error was occurred while inserting card node in method UpdateCardPriority! Reason: {0}, {1}";

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
                var card = writeResults.Select(x => new Card
                {
                    CardToken = x["c.card_token"].As<string>(),
                    FraudPriority = x["c.card_token"].As<double>(),
                    CardState = CardState.Default
                }).FirstOrDefault();
                
                returnResult.Result = card;
                returnResult.IsSuccessfully = true;
            }
            catch (Exception e)
            {
                FaultHandler.HandleError(ref returnResult, e, "Card node merging failed!", string.Format(errorMessageTemplate, e.Message, e.StackTrace));
            }
            finally
            {
                await session.CloseAsync();
            }

            return returnResult;
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