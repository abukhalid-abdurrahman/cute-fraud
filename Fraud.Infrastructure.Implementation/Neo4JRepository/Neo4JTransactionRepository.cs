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
    public class Neo4JTransactionRepository : ITransactionRepository
    {
        private readonly IDriver _driver;
        private bool _disposed = false;

        public Neo4JTransactionRepository(Neo4JConfigurations neo4JConfigurations)
        {
            _driver = GraphDatabase.Driver(neo4JConfigurations.Uri, 
                AuthTokens.Basic(neo4JConfigurations.UserName, neo4JConfigurations.Password));
        }

        public async Task<ReturnResult<bool>> Create(Transaction entity)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Neo4JTransactionRepository));
            const string query = @"
                MERGE (t:transaction { amount: $Amount, external_ref: $ExternalRef, sender_card_token: $SenderCardToken, receiver_card_token: $ReceiverCardToken, transaction_state: $TransactionState, date_created_unix: $DateCreatedUnix })
                RETURN t;";

            var session = _driver.AsyncSession();
            
            var returnResult = new ReturnResult<bool>();
            var errorMessageTemplate = "Error was occurred while inserting transaction node in method Create! Reason: {0}, {1}";
            try
            {
                await session.WriteTransactionAsync(async tx =>
                {
                    await tx.RunAsync(query, new
                    {
                        entity.Amount,
                        entity.SenderCardToken,
                        entity.ReceiverCardToken,
                        entity.ExternalRef,
                        TransactionState = (int) entity.TransactionState,
                        entity.DateCreatedUnix
                    });
                });
                
                returnResult.IsSuccessfully = true;
                returnResult.Result = true;
            }
            catch (Exception e)
            {
                FaultHandler.HandleError(ref returnResult, e, "Transactions node creating failed!", string.Format(errorMessageTemplate, e.Message, e.StackTrace));
            }
            finally
            {
                await session.CloseAsync();
            }
            return returnResult;
        }

        public async Task<ReturnResult<Transaction[]>> FindByDateRange(string cardToken, DateTimeOffset dateFrom, DateTimeOffset dateTo)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Neo4JTransactionRepository));
            
            const string query = @"
                MATCH (t:transaction)
                WHERE t.date_created_unix >= $DateFromUnix AND t.date_created_unix <= $DateToUnix AND t.sender_card_token = $SenderCardToken
                RETURN t";

            var session = _driver.AsyncSession();
            
            var returnResult = new ReturnResult<Transaction[]>();
            var errorMessageTemplate = "Error was occurred while fetching transactions in method FindByDateRange! Reason: {0}, {1}";
            
            try
            {
                var readResults = await session.ReadTransactionAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, 
                        new { DateFromUnix = dateFrom.ToUnixTimeSeconds(), DateToUnix = dateTo.ToUnixTimeSeconds(), CardToken = cardToken });
                    return await result.ToListAsync();
                });
                var transactionsByDateRange = readResults.Select(x => new Transaction
                {
                    Amount = x["t.amount"].As<uint>(),
                    SenderCardToken = x["t.sender_card_token"].As<string>(),
                    ReceiverCardToken = x["t.receiver_card_token"].As<string>(),
                    DateCreatedUnix = x["t.date_created_unix"].As<long>(),
                    ExternalRef = x["t.external_ref"].As<string>(),
                    TransactionState = (TransactionState)x["t.transaction_state"].As<int>()
                }).ToArray();

                returnResult.IsSuccessfully = true;
                returnResult.Result = transactionsByDateRange;
            }
            catch (Exception e)
            {
                FaultHandler.HandleError(ref returnResult, e, "Transactions node fetching failed!", string.Format(errorMessageTemplate, e.Message, e.StackTrace));
            }
            finally
            {
                await session.CloseAsync();
            }

            return returnResult;
        }

        public async Task<ReturnResult<Transaction[]>> FindLimit(string cardToken, int limit = 100)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Neo4JTransactionRepository));
            
            const string query = @"
                MATCH (t:transaction)
                WHERE t.sender_card_token = $SenderCardToken
                RETURN t
                LIMIT $Limit";

            var session = _driver.AsyncSession();
            
            var returnResult = new ReturnResult<Transaction[]>();
            var errorMessageTemplate = "Error was occurred while fetching transactions in method FindLimit! Reason: {0}, {1}";
            try
            {
                var readResults = await session.ReadTransactionAsync(async tx =>
                {
                    var result = await tx.RunAsync(query, new { Limit = limit, CardToken = cardToken });
                    return await result.ToListAsync();
                });
                var transactionByCardToken = readResults.Select(x => new Transaction
                {
                    Amount = x["t.amount"].As<uint>(),
                    SenderCardToken = x["t.sender_card_token"].As<string>(),
                    ReceiverCardToken = x["t.receiver_card_token"].As<string>(),
                    DateCreatedUnix = x["t.date_created_unix"].As<long>(),
                    ExternalRef = x["t.external_ref"].As<string>(),
                    TransactionState = (TransactionState)x["t.transaction_state"].As<int>()
                }).ToArray();
                
                returnResult.IsSuccessfully = true;
                returnResult.Result = transactionByCardToken;
            }
            catch (Exception e)
            {
                FaultHandler.HandleError(ref returnResult, e, "Transactions node fetching failed!", string.Format(errorMessageTemplate, e.Message, e.StackTrace));
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

        ~Neo4JTransactionRepository()
        {
            ReleaseUnmanagedResources();
        }
    }
}