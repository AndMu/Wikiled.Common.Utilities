using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Extensions.Logging;
using Wikiled.Common.Utilities.Config;

namespace Wikiled.Common.Utilities.Rx
{
    public class ObservableTimer : IObservableTimer
    {
        private readonly ILogger<ObservableTimer> logger;

        private readonly IApplicationConfiguration configuration;

        private readonly IScheduler scheduler;

        public ObservableTimer(ILogger<ObservableTimer> logger, IApplicationConfiguration configuration, IScheduler scheduler)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IObservable<long> Daily(params TimeSpan[] times)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (times == null)
            {
                throw new ArgumentNullException(nameof(times));
            }

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
