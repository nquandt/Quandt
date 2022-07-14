using System;
using System.IO;
using System.Threading.Tasks;

namespace Quandt.Abstractions
{
    public interface ISerializer
    {
        string ContentType { get; }
        T DeserializeSync<T>(Stream stream);
        void SerializeSync<T>(MemoryStream stream, T obj);

        Task<T> DeserializeAsync<T>(Stream stream);

        Task SerializeAsync<T>(MemoryStream stream, T obj);
    }
}
