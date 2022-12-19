using StockEngine.FilesProcessor.Utils;
using StockEngine.Interfaces;

namespace StockEngine.FilesProcessor
{
    internal class JsonProcessor : IProcessor
    {
        private IStockRepository _stockRepository;

        public JsonProcessor(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }
        public async Task<bool> UpdateStockAsync(string source)
        {
            try
            {
                //if remote
                if (!File.Exists(source))
                    return false;
                   //Log
                    
                    using (var jsonContent = File.OpenRead(source))
                        await _stockRepository.SaveTheLowerPricesAsync(JsonUtilReader.DeserializeFromStream(jsonContent));

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