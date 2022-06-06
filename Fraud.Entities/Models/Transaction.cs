using System;
using Fraud.Entities.DTOs.Order;
using Fraud.Entities.Enums;

namespace Fraud.Entities.Models
{
    public class Transaction
    {
        public uint Amount { get; set; }
        public string ExternalRef { get; set; }
        public string SenderCardToken { get; set; }
        public string ReceiverCardToken { get; set; }
        public long DateCreatedUnix { get; set; }
        public TransactionState TransactionState { get; set; } = TransactionState.Default;

        public Transaction() {}
        
        public Transaction(OrderRequest orderRequest)
        {
            if (orderRequest == null)
                throw new ArgumentNullException(nameof(orderRequest));
            
            ExternalRef = orderRequest.ExternalRef;
            SenderCardToken = orderRequest.Sender?.Token;
            ReceiverCardToken = orderRequest.Receiver?.Token;
            DateCreatedUnix = DateTimeOffset.Now.ToUnixTimeSeconds();
            Amount = orderRequest.Amount;
        }
    }
}