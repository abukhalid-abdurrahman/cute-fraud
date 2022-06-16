using Fraud.Concerns.Extensions;
using Fraud.Entities.Enums;

namespace Fraud.Entities.DTOs.Event
{
    public class EventTypeDto
    {
        public EventType EventCode { get; set; }
        public string EventName => EventCode.GetDescription();
    }
}