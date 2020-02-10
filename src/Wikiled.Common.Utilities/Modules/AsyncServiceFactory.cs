using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Modules
{
    public class AsyncServiceFactory<T> : IAsyncServiceFactory<T>
    {
        readonly IServiceProvider provider;
        private readonly Func<IServiceProvider, T, Task> init;

        public AsyncServiceFactory(IServiceProvider services, Func<IServiceProvider, T, Task> init)
        {
            this.provider = services ?? throw new ArgumentNullException(nameof(services));
            this.init = init ?? throw new ArgumentNullException(nameof(init));
        }

        public async Task<T> GetService()
        {
            var service = provider.GetRequiredService<T>();
            await init(provider, service).ConfigureAwait(false);
            return service;
        }
    }
}
