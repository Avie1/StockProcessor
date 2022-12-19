using Newtonsoft.Json.Linq;
using StockEngine.Interfaces;
using StockEngine.Models;
using System.Collections;

namespace StockEngine.FilesProcessor
{
    internal class CsvProcessor : IProcessor
    {
        private readonly IStockRepository _stockRepository;

        public CsvProcessor(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }
        public async Task<bool> UpdateStockAsync(string source)
        {
            try
            {
                if (!File.Exists(source))
                    return false;
                // log
                        
                    using (StreamReader reader = new StreamReader(File.OpenRead(source)))
                        await _stockRepository.SaveTheLowerPricesAsync(StreamStock(reader));

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Failed update lower prices", e);
                //Log
            }
        }

        private static async IAsyncEnumerable<Stock> StreamStock(StreamReader reader)
        {
            //skiping the header (first line)
            await reader.ReadLineAsync();

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (!string.IsNullOrEmpty(line))
                {
                    string[] values = line.Split(',');

                    Stock stock = new Stock
                    {
                        Name = values[0].ToString(),
                        Price = Convert.ToDecimal(values[2])
                    };
                    yield return stock;
                }

            }
        }
    }
}