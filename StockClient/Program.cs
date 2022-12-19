using StockEngine.Models;


StockEngine.StockProcessor stockProcessor = new StockEngine.StockProcessor();
List<string> sources = new List<string>()
{
    @"C:\personal\test\AccessFintech Backend Test\stocks.json",
    @"C:\personal\test\AccessFintech Backend Test\stocks.csv"
};
stockProcessor.StartProcess(sources);

for (int i = 0; i < 2; i++)
{
    foreach (var stock in stockProcessor.GetAllLowestPrices())
    {
        Console.WriteLine($"Stock Name:{stock.Name}, Stock Price:{stock.Price}");
    }
    Thread.Sleep(TimeSpan.FromSeconds(5));
}

var stock1 =stockProcessor.GetLowestPrice("AABA");
Console.WriteLine($"AABA price is {stock1.Price}");
stockProcessor.Stop();


