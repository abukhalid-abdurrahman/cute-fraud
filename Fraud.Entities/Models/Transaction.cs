using Fraud.Entities.Enums;

namespace Fraud.Entities.Models
{
    public class Transaction
    {
        public int Amount { get; set; }
        public string ExternalRef { get; set; }
        public string CardToken { get; set; }
        public long DateCreatedUnix { get; set; }
        public TransactionState TransactionState { get; set; } = TransactionState.Default;
    }
}