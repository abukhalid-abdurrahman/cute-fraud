using System.ComponentModel.DataAnnotations;
using Fraud.Concerns.Extensions;
using Fraud.Entities.Enums;

namespace Fraud.Entities.DTOs.Event
{
    public class EventTypeDto
    {
        [Required]
        public int EventId { get; set; }
        [Required(ErrorMessage = "Please choose one of state event!")]
        public EventType EventCode { get; set; }
        public string EventName => EventCode.GetDescription();
    }
}