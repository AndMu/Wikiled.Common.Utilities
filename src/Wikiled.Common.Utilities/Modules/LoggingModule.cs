using System;
using Autofac;
using Microsoft.Extensions.Logging;
using Wikiled.Common.Logging;

namespace Wikiled.Common.Utilities.Modules
{
    public class LoggingModule : Module
    {
        private readonly ILoggerFactory factory;

        private readonly ILogger<LoggingModule> logger;

        public LoggingModule()
            : this(ApplicationLogging.LoggerFactory)
        {
        }

        public LoggingModule(ILoggerFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            logger = factory.CreateLogger<LoggingModule>();
        }

        protected override void Load(ContainerBuilder builder)
        {
            var registration = builder.RegisterInstance(factory);
            if (factory == ApplicationLogging.LoggerFactory)
            {
                logger.LogDebug("Using external logger - disabling dispose");
                registration.ExternallyOwned();
            }
        }
    }
}
