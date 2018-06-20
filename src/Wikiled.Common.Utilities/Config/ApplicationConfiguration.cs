using System;

namespace Wikiled.Common.Utilities.Config
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public DateTime Now => DateTime.UtcNow;

        public DateTime GetWorkDay(DateTime monitorDate)
        {
            if (monitorDate.DayOfWeek == DayOfWeek.Saturday)
            {
                return monitorDate.AddDays(2);
            }

            return monitorDate.DayOfWeek == DayOfWeek.Sunday ? monitorDate.AddDays(1) : monitorDate;
        }

        public bool IsWorkDay(DateTime monitorDate)
        {
            return monitorDate.DayOfWeek != DayOfWeek.Saturday &&
                   monitorDate.DayOfWeek != DayOfWeek.Sunday;
        }

        public static string GetEnvironmentVariable(string key)
        {
            var value = Environment.GetEnvironmentVariable(key);
            if (value != null)
            {
                return value;
            }

            value = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);
            if (value != null)
            {
                return value;
            }

            return Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User);
        }

    }
}
