using System.Collections.Generic;
using Fraud.Entities.Models;
using Newtonsoft.Json;

namespace Fraud.Entities.DTOs.Order
{
    public class OrderRequest
    {
        [JsonProperty("external_ref")]
        public string ExternalRef { get; set; }
        [JsonProperty("sender")]
        public ParticipantOrderRequest Sender { get; set; }
        [JsonProperty("receiver")]
        public ParticipantOrderRequest Receiver { get; set; }
        [JsonProperty("amount")]
        public uint Amount { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("additional_params")]
        public Dictionary<string, string> AdditionalParams { get; set; }
        
        public static implicit operator Transaction(OrderRequest orderRequest) => new(orderRequest);
    }
}