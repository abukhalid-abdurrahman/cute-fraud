using System;

namespace Fraud.Concerns
{
    public static class DateUtils
    {
        public static DateTimeOffset GetStartDate()
        {
            return DateTimeOffset.Now.Date;
        }

        public static DateTimeOffset GetEndOfTheDate()
        {
            return DateTimeOffset.Now.Date.AddHours(23).AddMinutes(0).AddSeconds(0);
        }
    }
}