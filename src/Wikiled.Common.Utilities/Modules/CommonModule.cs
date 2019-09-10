using System.Reactive.Concurrency;
using Microsoft.Extensions.DependencyInjection;
using Wikiled.Common.Utilities.Config;
using Wikiled.Common.Utilities.Performance;
using Wikiled.Common.Utilities.Rx;

namespace Wikiled.Common.Utilities.Modules
{
    public class CommonModule : IModule
    {
        public void ConfigureServices(IServiceCollection service)
        {
            service.AddSingleton<IScheduler>(TaskPoolScheduler.Default);
            service.AddTransient<IApplicationConfiguration, ApplicationConfiguration>();
            service.AddTransient<IObservableTimer, ObservableTimer>();
            service.AddTransient<ISystemUsageCollector, SystemUsageCollector>();
            service.AddTransient<ISystemUsageMonitor, SystemUsageMonitor>();
            service.AddTransient<ISystemUsageBucket, SystemUsageBucket>();
        }
    }
}
