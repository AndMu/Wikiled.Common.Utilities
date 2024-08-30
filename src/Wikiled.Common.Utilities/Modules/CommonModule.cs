using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using System.Reactive.Concurrency;
using Wikiled.Common.Utilities.Config;
using Wikiled.Common.Utilities.Performance;
using Wikiled.Common.Utilities.Resources;
using Wikiled.Common.Utilities.Rx;
using Wikiled.Common.Utilities.Serialization;

namespace Wikiled.Common.Utilities.Modules;

public static class CommonModule
{
    public static IServiceCollection AddCommonServices(this IServiceCollection service)
    {
        service.AddSingleton<IScheduler>(TaskPoolScheduler.Default);
        service.AddSingleton<RecyclableMemoryStreamManager>();
        service.AddSingleton<IDataDownloader, DataDownloader>();
        service.AddTransient<IJsonSerializer, BasicJsonSerializer>();
        service.AddTransient<IApplicationConfiguration, ApplicationConfiguration>();
        service.AddTransient<IObservableTimer, ObservableTimer>();
        service.AddTransient<ISystemUsageCollector, SystemUsageCollector>();
        service.AddTransient<ISystemUsageMonitor, SystemUsageMonitor>();
        service.AddTransient<ISystemUsageBucket, SystemUsageBucket>();
        service.AddTransient(typeof(IServiceFactory<>), typeof(ContainerServiceFactory<>));
        return service;
    }
}