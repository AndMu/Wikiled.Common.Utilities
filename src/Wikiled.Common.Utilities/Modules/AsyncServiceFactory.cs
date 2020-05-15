using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Modules
{
    public class AsyncServiceFactory<T> : IAsyncServiceFactory<T>
    {
        readonly IServiceProvider provider;
        private readonly Func<IServiceProvider, T, Task> init;
        private readonly Func<IServiceProvider, Task<T>> construct;

        private Lazy<Task<T>> cache;

        public AsyncServiceFactory(IServiceProvider services, Func<IServiceProvider, T, Task> init)
        {
            provider = services ?? throw new ArgumentNullException(nameof(services));
            this.init = init ?? throw new ArgumentNullException(nameof(init));
        }

        public AsyncServiceFactory(IServiceProvider services, Func<IServiceProvider, Task<T>> construct)
        {
            provider = services ?? throw new ArgumentNullException(nameof(services));
            this.construct = construct ?? throw new ArgumentNullException(nameof(init));
        }

        public Task<T> GetService(bool refresh = false)
        {
            if (refresh ||
                cache == null)
            {
                cache = new Lazy<Task<T>>(GetServiceInternal);
            }

            return cache.Value;
        }

        private async Task<T> GetServiceInternal()
        {
            if (construct == null)
            {
                var service = provider.GetRequiredService<T>();
                await init(provider, service).ConfigureAwait(false);
                return service;
            }

            return await construct(provider).ConfigureAwait(false);
        }
    }
}
