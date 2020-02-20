using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wikiled.Common.Logging;
using Wikiled.Common.Utilities.Helpers;

namespace Wikiled.Common.Utilities.Modules
{
    public class LoggingModule : IModule
    {
        private readonly ILoggerFactory factory;

        public LoggingModule()
            : this(new WrappedLoggerFactory(ApplicationLogging.LoggerFactory))
        {
        }

        public LoggingModule(ILoggerFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var logger = factory.CreateLogger<LoggingModule>();
            services.AddSingleton(factory);
            services.AddLogging();
            logger.LogDebug("Setting logging module");
            return services;
        }
    }
}
