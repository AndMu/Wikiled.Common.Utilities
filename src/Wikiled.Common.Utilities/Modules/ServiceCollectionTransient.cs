using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Wikiled.Common.Utilities.Modules
{
    public static class ServiceCollectionTransient
    {
        public static IServiceCollection AddTransient<TService, TMetadata>(this IServiceCollection services, TMetadata metadata)
            where TService : class
        {
            return services.AddTransient<TService, TService, TMetadata>(metadata);
        }

        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory, string context)
            where TService : class
            where TImplementation : class, TService
        {
            return services.AddTransient<TService, TImplementation, string>(implementationFactory, context);
        }

        public static IServiceCollection AddTransient<TService, TImplementation, TMetadata>(
            this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory,
            TMetadata metadata)
            where TService : class
            where TImplementation : class, TService
        {
            if (metadata == null) throw new ArgumentNullException(nameof(metadata));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));

            return services.AddTransient<TService>(
                               s => s.GetServices<IServiceMetadata<TService, TMetadata>>()
                                     .OfType<IServiceMetadata<TService, TImplementation, TMetadata>>()
                                     .First(x => Equals(x.Metadata, metadata))
                                     .CachingImplementationFactory(s)) // This registration ensures that only one instance is created in the scope
                           .AddTransient(
                               (Func<IServiceProvider, IServiceMetadata<TService, TMetadata>>)
                               (s => new ServiceMetadata<TService, TImplementation, TMetadata>(metadata, implementationFactory)));
        }

        public static IServiceCollection AddTransient<TService, TImplementation, TMetadata>(
            this IServiceCollection services,
            TImplementation implementation,
            TMetadata metadata)
            where TService : class
            where TImplementation : class, TService
        {
            if (metadata == null) throw new ArgumentNullException(nameof(metadata));

            return services.AddTransient<TService>(s => s.GetServices<IServiceMetadata<TService, TMetadata>>()
                                                      .OfType<IServiceMetadata<TService, TImplementation, TMetadata>>()
                                                      .First(sm => Equals(sm.Metadata, metadata))
                                                      .CachingImplementationFactory(s))
                           .AddTransient<IServiceMetadata<TService, TMetadata>>(
                               s => new ServiceMetadata<TService, TImplementation, TMetadata>(metadata, ss => implementation));
        }

        public static IServiceCollection AddTransient<TService, TImplementation, TMetadata>(this IServiceCollection services, TMetadata metadata)
            where TService : class
            where TImplementation : class, TService
        {
            if (metadata == null) throw new ArgumentNullException(nameof(metadata));

            services.TryAddTransient<TImplementation>();

            if (typeof(TService) != typeof(TImplementation))
            {
                services.AddTransient<TService>(s => s.GetServices<IServiceMetadata<TService, TMetadata>>()
                                                   .OfType<IServiceMetadata<TService, TImplementation, TMetadata>>()
                                                   .First(sm => Equals(sm.Metadata, metadata))
                                                   .CachingImplementationFactory(s));
            }

            return services.AddTransient<IServiceMetadata<TService, TMetadata>>
                (s => new ServiceMetadata<TService, TImplementation, TMetadata>(metadata, ss => ss.GetService<TImplementation>()));
        }

        public static IServiceCollection AddTransient<TService>(this IServiceCollection services, string context)
            where TService : class
        {
            return services.AddTransient<TService, string>(context);
        }

        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services, string context)
            where TService : class
            where TImplementation : class, TService
        {
            return services.AddTransient<TService, TImplementation, string>(context);
        }
    }
}
