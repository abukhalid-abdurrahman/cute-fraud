using System.ComponentModel;

namespace Fraud.Entities.Enums
{
    public enum ActionType
    {
        [Description("Temporary Block")]
        TemporaryBlockAction = 0,
        [Description("Block")]
        BlockAction = 1,
        [Description("Verify")]
        RequestVerification = 2,
    }
}