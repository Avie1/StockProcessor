namespace StockEngine.Interfaces
{
    /// <summary>
    /// Processor fetch
    /// </summary>
    internal interface IProcessor
    {
        Task<bool> UpdateStockAsync(string source);
    }
}