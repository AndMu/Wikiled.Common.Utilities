using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using NLog;
using Wikiled.Common.Arguments;
using Wikiled.Common.Utilities.Config;

namespace Wikiled.Common.Utilities.Rx
{
    public class ObservableTimer
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IApplicationConfiguration configuration;

        public ObservableTimer(IApplicationConfiguration configuration)
        {
            Guard.NotNull(() => configuration, configuration);
            this.configuration = configuration;
        }

        public IObservable<long> Daily(params TimeSpan[] times)
        {
            Guard.NotNull(() => times, times);
            return Daily(Scheduler.Default, times);
        }

        public IObservable<long> Daily(IScheduler scheduler, params TimeSpan[] times)
        {
            Guard.NotNull(() => scheduler, scheduler);
            Guard.NotNull(() => times, times);

            if (times.Length == 0)
            {
                return Observable.Never<long>();
            }

            var sortedTimes = times.ToList();
            sortedTimes.Sort();
            return Observable.Defer(
                                 () =>
                                 {
                                     var now = configuration.Now.AddMinutes(1);
                                     var next = sortedTimes.FirstOrDefault(time => now.TimeOfDay < time);

                                     var date = next > TimeSpan.Zero
                                                    ? now.Date.Add(next)
                                                    : configuration.GetWorkDay(now.Date.AddDays(1)).Add(sortedTimes[0]);

                                     logger.Info($"Next @{date} from {sortedTimes.Aggregate("", (s, t) => s + t + ", ")}");
                                     return Observable.Timer(date, scheduler);
                                 })
                             .Repeat()
                             .Scan(-1L, (n, _) => n + 1);
        }
    }
}
