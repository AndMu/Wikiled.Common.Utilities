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

        public async Task<T> GetService()
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
