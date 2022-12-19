
using Newtonsoft.Json;
using StockEngine.Models;

namespace StockEngine.FilesProcessor.Utils
{
    internal class JsonUtilReader
    {
        internal static async IAsyncEnumerable<Stock> DeserializeFromStream(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                while (await jsonTextReader.ReadAsync())
                {
                    if (jsonTextReader.TokenType == JsonToken.StartObject)
                        yield return serializer.Deserialize<Stock>(jsonTextReader);

                }
            }
        }
    }
}