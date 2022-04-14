namespace Fraud.Concerns.Configurations
{
    public class RabbitMqConfigurations
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string TransactionQueueName { get; set; }

        public string BlockCardExchangeName { get; set; }
        public string TemporaryBlockCardExchangeName { get; set; }
        public string SuspensionsCardExchangeName { get; set; }
        
        public string BlockCardRoutingKey { get; set; }
        public string TemporaryBlockCardRoutingKey { get; set; }
        public string SuspensionsCardRoutingKey { get; set; }
    }
}