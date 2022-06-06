namespace Fraud.Concerns.Configurations
{
    public class RabbitMqConfigurations : CredentialsConfigurationBase
    {
        public string CreditQueueName { get; set; }
        public string DebitQueueName { get; set; }
        public string P2PQueueName { get; set; }

        public string BlockCardExchangeName { get; set; }
        public string TemporaryBlockCardExchangeName { get; set; }
        public string SuspensionsCardExchangeName { get; set; }
        
        public string BlockCardRoutingKey { get; set; }
        public string TemporaryBlockCardRoutingKey { get; set; }
        public string SuspensionsCardRoutingKey { get; set; }
    }
}