using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using System.Reactive.Concurrency;
using Wikiled.Common.Utilities.Config;
using Wikiled.Common.Utilities.Performance;
using Wikiled.Common.Utilities.Resources;
using Wikiled.Common.Utilities.Resources.Config;
using Wikiled.Common.Utilities.Rx;
using Wikiled.Common.Utilities.Serialization;

namespace Wikiled.Common.Utilities.Modules
{
    public class CommonModule : IModule
    {
        public IServiceCollection ConfigureServices(IServiceCollection service)
        {
            service.AddSingleton<IScheduler>(TaskPoolScheduler.Default);
            service.AddSingleton<RecyclableMemoryStreamManager>();
            service.AddSingleton<IJsonStreamingWriterFactory, JsonStreamingWriterFactory>();
            service.AddSingleton<IDataDownloader, DataDownloader>();
            service.AddSingleton(typeof(ConfigDownloader<>));

            service.AddTransient<IJsonSerializer, BasicJsonSerializer>();
            service.AddTransient<IJObjectSerialiser, JObjectSerialiser>();
            service.AddTransient<IApplicationConfiguration, ApplicationConfiguration>();
            service.AddTransient<IObservableTimer, ObservableTimer>();
            service.AddTransient<ISystemUsageCollector, SystemUsageCollector>();
            service.AddTransient<ISystemUsageMonitor, SystemUsageMonitor>();
            service.AddTransient<ISystemUsageBucket, SystemUsageBucket>();
            service.AddTransient(typeof(IServiceFactory<>), typeof(ContainerServiceFactory<>));
            return service;
        }
    }
}
