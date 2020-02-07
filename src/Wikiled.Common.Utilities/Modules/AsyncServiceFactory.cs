using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Modules
{
    public class AsyncServiceFactory<T> : IAsyncServiceFactory<T>
    {
        readonly IServiceProvider services;
        private readonly Func<T, Task> init;

        public AsyncServiceFactory(IServiceProvider services, Func<T, Task> init)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.init = init ?? throw new ArgumentNullException(nameof(init));
        }

        public async Task<T> GetService()
        {
            var service = services.GetRequiredService<T>();
            await init(service).ConfigureAwait(false);
            return service;
        }
    }
}
