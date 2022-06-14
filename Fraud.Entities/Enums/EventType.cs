using System.ComponentModel;

namespace Fraud.Entities.Enums
{
    public enum EventType
    {
        [Description("Operations Above Average")]
        OperationsAboveAverageEvent = 0,
        [Description("Minimal Operation Interval")]
        MinimumIntervalEvent = 1,
        [Description("Amount Above Average")]
        AmountAboveAverageEvent = 2
    }
}