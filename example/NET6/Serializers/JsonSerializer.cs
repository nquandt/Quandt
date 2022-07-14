using Quandt.Abstractions;

namespace WebProject1.Serializers
{    
    [SupportsAdditionalContentTypes("text/json","rofl/json")]
    [SupportsQueryFormats("json")]
    public class JsonSerializer : ISerializer
    {
        public string ContentType => "application/json";

        public Task<T> DeserializeAsync<T>(Stream stream)
        {
            return Utf8Json.JsonSerializer.DeserializeAsync<T>(stream);
        }

        public T DeserializeSync<T>(Stream stream)
        {
            return Utf8Json.JsonSerializer.Deserialize<T>(stream);
        }

        public Task SerializeAsync<T>(MemoryStream stream, T obj)
        {
            return Utf8Json.JsonSerializer.SerializeAsync<T>(stream, obj);
        }

        public void SerializeSync<T>(MemoryStream stream, T obj)
        {
            Utf8Json.JsonSerializer.Serialize(stream, obj);
        }
    }
}
