using Fraud.Concerns.Extensions;
using Fraud.Entities.Enums;

namespace Fraud.Entities.DTOs.Action
{
    public class ActionTypeDto
    {
        public ActionType ActionCode { get; set; }
        public string ActionName => ActionCode.GetDescription();
    }
}