using StockEngine.FilesProcessor;
using StockEngine.Interfaces;
using StockEngine.Models;
using StockEngine.Repository;
using System.Threading;
using System.Timers;

namespace StockEngine
{
    public class StockProcessor
    {
        const double JOB_INTERVAL = 20000; //20 sec
        private List<System.Timers.Timer> _timers = new List<System.Timers.Timer>();
        private IStockRepository _stockRepository = new StockRepository();
        public void StartProcess(List<string> sources)
        {

            Dictionary<string, IProcessor> stockProcessors = MatchProcessors(sources);


            foreach (var item in stockProcessors)
            {
                System.Timers.Timer timer = new System.Timers.Timer
                {

                    Enabled = true
                };
                timer.Elapsed += async (sender, e) => await OnTimerStart(sender, e, item.Value, item.Key);
                timer.Start();
                _timers.Add(timer);
            }
        }


        public void Stop()
        {
            _timers.ForEach(x => x.Stop());

            _timers.Clear();
        }
        public Stock GetLowestPrice(string stockName)
        {
            return _stockRepository.GetLowestPrice(stockName);
        }

        public IEnumerable<Stock> GetAllLowestPrices()
        {
            return _stockRepository.GetAllLowestPrices();
        }

        static async Task<bool> OnTimerStart(object sender, ElapsedEventArgs e, IProcessor processor, string filePath)
        {
            //Stoping the timer to ignore overlaping
            var timer = ((System.Timers.Timer)sender);
            timer.Stop();
            var value = await processor.UpdateStockAsync(filePath);
            timer.Interval = JOB_INTERVAL;
            timer.Start();
            return value;
        }



        private Dictionary<string, IProcessor> MatchProcessors(List<string> sources)
        {
            // match the best result by the extention and url
            Dictionary<string, IProcessor> stockProcessors = new Dictionary<string, IProcessor>();
            foreach (var source in sources)
            {
                if (source.EndsWith("csv", StringComparison.OrdinalIgnoreCase))
                {
                    stockProcessors.Add(source, new CsvProcessor(_stockRepository));
                    continue;
                }
                if (Uri.IsWellFormedUriString(source, UriKind.Absolute) &&
                    source.EndsWith("json", StringComparison.OrdinalIgnoreCase))
                {
                    stockProcessors.Add(source, new RemoteJsonProcessor(_stockRepository));
                    continue;
                }
                if (source.EndsWith("json", StringComparison.OrdinalIgnoreCase))
                {
                    stockProcessors.Add(source, new JsonProcessor(_stockRepository));
                    continue;
                }
            }

            return stockProcessors;
        }

    }
}