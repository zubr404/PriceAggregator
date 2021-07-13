using Binance.Common.Kline;
using Binance.DataSource.Kline;
using Binance.StockExchange.Kline.REST;
using Binance.StockExchange.Kline.WS;
using PriceAggregator.Library.Percentage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PriceAggregator.Library
{
    public class PriceAggregatorManager
    {
        private readonly ExchangeInfo exchangeInfo;
        private readonly KlineReceiver klineReceiver;
        private readonly KlineStreamManager klineStreamManager;
        private readonly PercentageChangeService percentageChangeService;
        private readonly Thread threadPriceAggregator;

        private IEnumerable<string> simbols;
        private IEnumerable<string> intervals;

        public PriceAggregatorManager(IRepository repository)
        {
            exchangeInfo = new ExchangeInfo();
            Pairs = exchangeInfo.AllPairsMarket.MarketPairs.Select(x => x.Pair);
            klineReceiver = new KlineReceiver(repository);
            klineStreamManager = new KlineStreamManager(repository);
            percentageChangeService = new PercentageChangeService(repository);
            threadPriceAggregator = new Thread(processing);
        }

        public IEnumerable<string> Pairs { get; private set; }
        public List<PercentageChange> PercentageChanges { get; private set; }

        public async Task RunAsync(IEnumerable<string> simbols, IEnumerable<string> intervals)
        {
            klineStreamManager.ConnectStreams(simbols, KlineTimeframe.TimeframesAdaptive, CommonSettings.RESTART_STREAM_TIME, CommonSettings.INTERVAL_CHANNEL_RESTART);
            await klineReceiver.Get(simbols, KlineTimeframe.TimeframesAdaptive);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{DateTime.Now} Каналы подключены. Исторя загружена.");
            Console.ResetColor();

            this.simbols = simbols;
            this.intervals = intervals;
            threadPriceAggregator.Start();
        }

        public void ThreadAbort()
        {
            threadPriceAggregator.Abort();
            klineStreamManager.DisconnectStream();
        }

        private void processing()
        {
            while (true)
            {
                PercentageChanges = percentageChangeService.GetPercentages(simbols, intervals);
                Console.WriteLine($"{DateTime.Now} PercentageChanges count = {PercentageChanges.Count}");
                //foreach (var percentageChange in PercentageChanges)
                //{
                //    Console.WriteLine($"{DateTime.Now} {percentageChange.Simbol} {percentageChange.Interval} {percentageChange.Percentage}");
                //}
                Thread.Sleep(1000);
            }
        }
    }
}
