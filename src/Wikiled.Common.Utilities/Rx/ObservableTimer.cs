using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Extensions.Logging;
using Wikiled.Common.Arguments;
using Wikiled.Common.Utilities.Config;

namespace Wikiled.Common.Utilities.Rx
{
    public class ObservableTimer
    {
        private readonly ILogger<ObservableTimer> logger;

        private readonly IApplicationConfiguration configuration;

        public ObservableTimer(IApplicationConfiguration configuration, ILoggerFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            logger = factory.CreateLogger<ObservableTimer>();
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

                                     logger.LogInformation($"Next @{date} from {sortedTimes.Aggregate("", (s, t) => s + t + ", ")}");
                                     return Observable.Timer(date, scheduler);
                                 })
                             .Repeat()
                             .Scan(-1L, (n, _) => n + 1);
        }
    }
}
