using System;

namespace Fraud.Entities.Models
{
    public class Order
    {
        public string ExternalRef { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;
        public string Source { get; set; }
        public string Destination { get; set; }
    }
}