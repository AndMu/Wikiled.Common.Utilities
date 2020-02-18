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

        private readonly Action<ILoggingBuilder> buildAction;

        private static bool configured;

        public LoggingModule()
            : this(new WrappedLoggerFactory(ApplicationLogging.LoggerFactory))
        {
        }

        public LoggingModule(ILoggerFactory factory, Action<ILoggingBuilder> action = null)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            buildAction = action ?? (logBuilder => { logBuilder.SetMinimumLevel(LogLevel.Trace); });
        }

        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var logger = factory.CreateLogger<LoggingModule>();
            if (configured)
            {
                logger.LogWarning("Logging already configured");
                return services;
            }

            configured = true;
            services.AddSingleton(factory);
            services.AddLogging(buildAction);
            logger.LogDebug("Setting logging module");
            return services;
        }
    }
}
