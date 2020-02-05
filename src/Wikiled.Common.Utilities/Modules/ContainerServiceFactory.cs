using Microsoft.Extensions.DependencyInjection;
using System;

namespace Wikiled.Common.Utilities.Modules
{
    internal class ContainerServiceFactory<T> : IServiceFactory<T>
    {
        readonly IServiceProvider services;

        public ContainerServiceFactory(IServiceProvider services) => this.services = services;

        public T GetService() => services.GetRequiredService<T>();
    }
}
