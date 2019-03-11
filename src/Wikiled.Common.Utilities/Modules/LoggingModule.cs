using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wikiled.Common.Logging;
using Wikiled.Common.Utilities.Helpers;

namespace Wikiled.Common.Utilities.Modules
{
    public class LoggingModule : Module
    {
        private readonly IServiceCollection services = new ServiceCollection();

        public LoggingModule()
            : this(new WrappedLoggerFactory(ApplicationLogging.LoggerFactory))
        {
        }

        public LoggingModule(ILoggerFactory factory)
        {
            services.AddSingleton(factory);
            services.AddLogging(logBuilder =>
            {
                logBuilder.SetMinimumLevel(LogLevel.Trace);
            });
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Populate(services);
        }
    }
}
