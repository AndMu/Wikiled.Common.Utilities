using System;

namespace Wikiled.Common.Utilities.Serialization
{
    public interface IJsonStreamingWriter
        : IDisposable
    {
        void WriteObject<T>(T instance);
    }
}