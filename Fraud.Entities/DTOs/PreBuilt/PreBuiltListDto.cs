using System.Collections.Generic;
using Fraud.Entities.DTOs.Action;
using Fraud.Entities.DTOs.Event;

namespace Fraud.Entities.DTOs.PreBuilt
{
    public class PreBuiltListDto
    {
        public List<ActionTypeDto> Actions { get; set; }
        public List<EventTypeDto> Events { get; set; }
    }
}