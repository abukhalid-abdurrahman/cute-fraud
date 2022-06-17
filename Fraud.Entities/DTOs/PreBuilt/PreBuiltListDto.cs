using Fraud.Entities.DTOs.Action;
using Fraud.Entities.DTOs.Event;

namespace Fraud.Entities.DTOs.PreBuilt
{
    public class PreBuiltListDto
    {
        public ActionTypeDto[] Actions { get; set; }
        public EventTypeDto[] Events { get; set; }
    }
}