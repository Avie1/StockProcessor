using StockEngine.FilesProcessor.Utils;
using StockEngine.Interfaces;

namespace StockEngine.FilesProcessor
{
    internal class RemoteJsonProcessor : IProcessor
    {
        private IStockRepository _stockRepository;

        public RemoteJsonProcessor(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }
        public async Task<bool> UpdateStockAsync(string source)
        {
            HttpClient httpClient = new HttpClient();
            var httpResult = await httpClient.GetAsync(source);
            var content = await httpResult.Content.ReadAsStreamAsync();
            try
            {
                await _stockRepository.SaveTheLowerPricesAsync(JsonUtilReader.DeserializeFromStream(content));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Failed update lower prices", e);
                //Log
            }
        }
    }
}