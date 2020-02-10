using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Modules
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTransientWithFactory<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddTransient<TService, TImplementation>();
            return services.AddSingleton<Func<TService>>(x => x.GetService<TService>);
        }

        public static IServiceCollection AddFactory<TService>(this IServiceCollection services)
            where TService : class
        {
            return services.AddSingleton<Func<TService>>(x => x.GetService<TService>);
        }

        public static IServiceCollection AddAsyncFactory<TService>(this IServiceCollection services, Func<IServiceProvider, TService, Task> init)
            where TService : class
        {
            return services.AddSingleton<IAsyncServiceFactory<TService>>(x => new AsyncServiceFactory<TService>(x, init));
        }

        public static IServiceCollection AddFactory<TParent, TChild>(this IServiceCollection services)
            where TChild : TParent
            where TParent : class
        {
            return services.AddSingleton<Func<TParent>>(x => () => (TParent)x.GetService<TChild>());
        }

        public static IServiceCollection As<TParent, TChild>(this IServiceCollection services, Action<TChild> onActivating = null)
            where TChild : TParent
            where TParent : class
        {
            return services.AsTransient<TParent, TChild>(onActivating);
        }

        public static IServiceCollection AsTransient<TParent, TChild>(this IServiceCollection services, Action<TChild> onActivating)
            where TChild : TParent
            where TParent : class
        {
            services.AddTransient<TParent>(x =>
            {
                var service = x.GetService<TChild>();
                onActivating?.Invoke(service);

                return service;
            });

            return services;
        }

        public static IServiceCollection AsScoped<TParent, TChild>(this IServiceCollection services, Action<TChild> onActivating)
            where TChild : TParent
            where TParent : class
        {
            services.AddScoped<TParent>(x =>
            {
                var service = x.GetService<TChild>();
                onActivating?.Invoke(service);

                return service;
            });

            return services;
        }

        public static IServiceCollection AsSingleton<TParent, TChild>(this IServiceCollection services, Action<TChild> onActivating)
            where TChild : TParent
            where TParent : class
        {
            services.AddSingleton<TParent>(x =>
            {
                var service = x.GetService<TChild>();
                onActivating?.Invoke(service);
                return service;
            });

            return services;
        }

        public static IServiceCollection RegisterModule<TModule>(this IServiceCollection services)
          where TModule : IModule, new()
        {
            return new TModule().ConfigureServices(services);
        }

        public static IServiceCollection RegisterModule<TModule>(this IServiceCollection services, TModule module)
            where TModule : IModule
        {
            return module.ConfigureServices(services);
        }
    }
}
