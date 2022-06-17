using Fraud.Entities.Enums;

namespace Fraud.Entities.Models
{
    public class Events
    {
        public int Id { get; set; }
        public EventType EventCode { get; set; }
    }
}