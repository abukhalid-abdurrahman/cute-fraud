using System.ComponentModel;

namespace Fraud.Entities.Enums
{
    public enum EventType
    {
        [Description("Operations Above Average")]
        NumberOfOperationsAboveAverageEvent = 0,
        [Description("Minimal Operation Interval")]
        MinimumOperationIntervalEvent = 1,
        [Description("Amount Above Average")]
        AmountOfOperationsAboveAverageEvent = 2
    }
}