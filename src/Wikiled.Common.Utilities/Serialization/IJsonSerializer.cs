﻿using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Wikiled.Common.Utilities.Serialization
{
    public interface IJsonSerializer
    {
        JsonSerializerOptions Options { get; }

        Task<MemoryStream> Serialize<T>(T instance);

        byte[] SerializeArray<T>(T instance);

        ValueTask<T> Deserialize<T>(Stream stream);

        T Deserialize<T>(byte[] data);

        object Deserialize(byte[] data, Type type);

        T Deserialize<T>(ArraySegment<byte> buffer);

        T Deserialize<T>(string json);

        ValueTask<T> DeserializeJsonZip<T>(string fileName);

        Task SerializeJsonZip<T>(T instance, string fileName);
    }
}
