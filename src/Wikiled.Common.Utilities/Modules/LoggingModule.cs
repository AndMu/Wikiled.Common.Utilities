using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Wikiled.Common.Utilities.Modules
{
    public class LoggingModule : Module
    {
        private readonly IServiceCollection services = new ServiceCollection();

        public LoggingModule(ILoggerFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

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
