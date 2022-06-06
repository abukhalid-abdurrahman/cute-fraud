using Newtonsoft.Json;

namespace Fraud.Entities.DTOs.Order
{
    public class ParticipantOrderRequest
    {
        [JsonProperty("account")]
        public string Account { get; set; }
        [JsonProperty("pan")]
        public string Pan { get; set; }
        [JsonProperty("expiry")]
        public string Expiry { get; set; }
        [JsonProperty("cvv2")]
        public string Cvv2 { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}