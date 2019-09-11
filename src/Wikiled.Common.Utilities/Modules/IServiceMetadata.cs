using System;

namespace Wikiled.Common.Utilities.Modules
{
    internal interface IServiceMetadata<out TService, out TMetadata>
    {
        Func<IServiceProvider, TService> CachingImplementationFactory { get; }

        TMetadata Metadata { get; }
    }

    internal interface IServiceMetadata<out TService, out TImplementation, out TMetadata> : IServiceMetadata<TService, TMetadata>
        where TService : class
        where TImplementation : class, TService
    {
        new Func<IServiceProvider, TImplementation> CachingImplementationFactory { get; }
    }
}
