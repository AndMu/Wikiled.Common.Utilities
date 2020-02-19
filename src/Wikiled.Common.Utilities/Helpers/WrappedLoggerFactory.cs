using Microsoft.Extensions.Logging;
using System;

namespace Wikiled.Common.Utilities.Helpers
{
    /// <summary>
    ///  Require so DI does not dispose static global one
    /// </summary>
    public class WrappedLoggerFactory : ILoggerFactory
    {
        private readonly ILoggerFactory inner;

        public WrappedLoggerFactory(ILoggerFactory inner)
        {
            this.inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return inner.CreateLogger(categoryName);
        }

        public void AddProvider(ILoggerProvider provider)
        {
            inner.AddProvider(provider); 
        }
    }
}
