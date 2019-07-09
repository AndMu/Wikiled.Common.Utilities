namespace Wikiled.Common.Utilities.Performance
{
    public static class SystemUsageCollectorExtension
    {
        public static string GetBasic(this ISystemUsage collector)
        {
            if (collector == null)
            {
                return string.Empty;
            }

            var result = (double)collector.WorkingSet / 1024;
            var type = "KB";
            if (result > 1024)
            {
                result /= 1024;
                type = "MB";

                if (result > 1024)
                {
                    result /= 1024;
                    type = "GB";
                }
            }

            return $"Working Set: {result:F2} {type} Total CPU Used: {collector.TotalCpuUsed:F2} User CPU: {collector.UserCpuUsed:F2}";
        }
    }
}
