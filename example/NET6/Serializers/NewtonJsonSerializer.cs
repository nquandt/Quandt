using Newtonsoft.Json;
using Quandt.Abstractions;
using System.Text;

namespace WebProject1.Serializers
{    
    [SupportsAdditionalContentTypes("nottext/json")]
    [SupportsQueryFormats("js2")]
    public class NewtsonJsonSerializer : ISerializer
    {
        public string ContentType => "random/json";
        public Task<T> DeserializeAsync<T>(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();

            var obj = JsonConvert.DeserializeObject<T>(text)!;

            return Task.FromResult<T>(obj);
        }

        public T DeserializeSync<T>(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();

            var obj = JsonConvert.DeserializeObject<T>(text)!;

            return obj;
        }

        public async Task SerializeAsync<T>(MemoryStream stream, T obj)
        {
            var str = JsonConvert.SerializeObject(obj);

            byte[] byteArray = Encoding.ASCII.GetBytes(str);
            MemoryStream memstream = new MemoryStream(byteArray);


            await memstream.CopyToAsync(stream);
        }

        public void SerializeSync<T>(MemoryStream stream, T obj)
        {
            var str = JsonConvert.SerializeObject(obj);

            byte[] byteArray = Encoding.ASCII.GetBytes(str);
            MemoryStream memstream = new MemoryStream(byteArray);


            memstream.CopyTo(stream);
        }
    }
}
