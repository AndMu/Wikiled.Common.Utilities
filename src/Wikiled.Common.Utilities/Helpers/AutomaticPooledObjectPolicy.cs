using Microsoft.Extensions.ObjectPool;
using System;

namespace Wikiled.Common.Utilities.Helpers
{
    public class AutomaticPooledObjectPolicy<T> : IPooledObjectPolicy<T>
    {
        private readonly Func<T> factory;

        private readonly Func<T, bool> clean;

        public AutomaticPooledObjectPolicy(Func<T> factory, Func<T, bool> clean)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.clean = clean ?? throw new ArgumentNullException(nameof(clean));
        }

        public T Create()
        {
            return factory();
        }

        public bool Return(T obj)
        {
            return clean(obj);
        }
    }
}
