using System.Collections.Concurrent;
using StockEngine.Interfaces;
using StockEngine.Models;

namespace StockEngine.Repository
{
    internal class StockRepository : IStockRepository
    {
        private static readonly ConcurrentDictionary<string, Stock> _lowerPrices = new ConcurrentDictionary<string, Stock>();

        public async Task SaveTheLowerPricesAsync(IAsyncEnumerable<Stock> stockList)
        {

            await foreach (var newStock in stockList)
            {

                //storedStock will retrive the same object from mem
                var storedStock = _lowerPrices.GetOrAdd(newStock.Name, newStock);

                lock (storedStock)
                {
                    if (storedStock.Price > newStock.Price)
                    {
                        storedStock.Price = newStock.Price;
                    }
                }
            }

        }

        public Stock GetLowestPrice(string stockName)
        {
            _lowerPrices.TryGetValue(stockName, out Stock returnValue);
            return returnValue;
        }
        public IEnumerable<Stock> GetAllLowestPrices()
        {
            return _lowerPrices.Values;
        }

    }
}