namespace Fraud.Entities.Enums
{
    public enum CardState
    {
        Default = 0,
        PreSuspicious = 1,
        Suspicious = 2,
        TemporaryBlocked = 3,
        Blocked = 4
    }
}