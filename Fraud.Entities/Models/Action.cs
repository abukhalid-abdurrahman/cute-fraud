using Fraud.Entities.Enums;

namespace Fraud.Entities.Models
{
    public class Action
    {
        public int Id { get; set; }
        public ActionType ActionCode { get; set; }
    }
}