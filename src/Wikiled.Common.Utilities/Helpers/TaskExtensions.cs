using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Helpers
{
    public static class TaskExtensions
    {
        public static void ForgetOrThrow(this Task task, ILogger logger)
        {
            task.ContinueWith(t => { logger.LogError(t.Exception, "Error"); }, TaskContinuationOptions.OnlyOnFaulted);
        }

    }
}
