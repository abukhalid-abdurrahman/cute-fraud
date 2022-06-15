using System.ComponentModel.DataAnnotations;
using Fraud.Concerns.Extensions;
using Fraud.Entities.Enums;

namespace Fraud.Entities.DTOs.Scenario
{
    public class StateEdgeDto
    {
        [Required(ErrorMessage = "Edge can't be without main state!")]
        public int FromStateId { get; set; }
        [Required(ErrorMessage = "Please link the state!")]
        public int ToStateId { get; set; }
        [Required(ErrorMessage = "Please provide state event name!")]
        public EventType EventType { get; set; }
        public string EventName => EventType.GetDescription();
    }
}