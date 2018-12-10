using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wikiled.Common.Logging;

namespace Wikiled.Common.Utilities.Modules
{
    public class LoggingModule : Module
    {
        private readonly IServiceCollection services = new ServiceCollection();

        public LoggingModule()
            : this(ApplicationLogging.LoggerFactory)
        {
        }

        public LoggingModule(ILoggerFactory factory)
        {
            ApplicationLogging.LoggerFactory = factory ?? throw new ArgumentNullException(nameof(factory));
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
