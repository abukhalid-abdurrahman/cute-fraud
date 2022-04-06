namespace Fraud.Entities.DTOs
{
    public struct TransactionAnalyzerResult
    {
        public TransactionAnalyzerResult(float fraudPriority, string cardToken)
        {
            FraudPriority = fraudPriority;
            CardToken = cardToken;
        }

        public float FraudPriority { get; set; }
        public string CardToken { get; set; }
    }
}