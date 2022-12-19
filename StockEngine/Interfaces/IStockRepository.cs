using StockEngine.Models;

namespace StockEngine.Interfaces
{
    internal interface IStockRepository
    {
        IEnumerable<Stock> GetAllLowestPrices();
        Stock GetLowestPrice(string stockName);
        Task SaveTheLowerPricesAsync(IAsyncEnumerable<Stock> stockList);
    }
}