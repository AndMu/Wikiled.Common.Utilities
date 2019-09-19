namespace Wikiled.Common.Utilities.Serialization
{
    public interface IJsonStreamingWriterFactory
    {
        IJsonStreamingWriter CreateJson(string path);

        IJsonStreamingWriter CreateCompressedJson(string path);
    }
}