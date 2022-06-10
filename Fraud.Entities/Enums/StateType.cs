using System.ComponentModel;

namespace Fraud.Entities.Enums
{
    public enum StateType
    {
        [Description("Initial")]
        Initial = 0,
        [Description("Custom")]
        Custom = 1,
        [Description("Final")]
        Final = 2
    }
}