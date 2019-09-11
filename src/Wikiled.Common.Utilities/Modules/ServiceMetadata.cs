using System;

namespace Wikiled.Common.Utilities.Modules
{
    internal class ServiceMetadata<TService, TImplementation, TMetadata> : IServiceMetadata<TService, TImplementation, TMetadata>
        where TService : class
        where TImplementation : class, TService
    {

        private TImplementation cachedImplementation;

        Func<IServiceProvider, TService> IServiceMetadata<TService, TMetadata>.CachingImplementationFactory => CachingImplementationFactory;

        public Func<IServiceProvider, TImplementation> CachingImplementationFactory { get; }

        public TMetadata Metadata { get; }

        public ServiceMetadata(TMetadata metadata, Func<IServiceProvider, TImplementation> implementationFactory)
        {
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            Metadata = metadata;
            CachingImplementationFactory = s => cachedImplementation ?? (cachedImplementation = implementationFactory(s));
        }
    }
}
