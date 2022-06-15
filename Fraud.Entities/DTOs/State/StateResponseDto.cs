using Fraud.Concerns.Extensions;
using Fraud.Entities.DTOs.Action;
using Fraud.Entities.Enums;

namespace Fraud.Entities.DTOs.State
{
    public class StateResponseDto
    {
        public StateVertexDto PreviousStateVertex { get; set; }
        public StateVertexDto[] NextStates { get; set; }
        public string Name { get; set; }
        public string StateTypeName => StateType.GetDescription();
        public StateType StateType { get; set; }
        public ActionTypeDto[] ActionTypes { get; set; }
        public EventType EventType { get; set; }
        public string EventTypeName => EventType.GetDescription();
        public int ExpirationTime { get; set; }
    }
}