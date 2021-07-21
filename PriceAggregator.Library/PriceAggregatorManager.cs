using Binance.Common.Kline;
using Binance.DataSource.Kline;
using Binance.StockExchange.Kline.REST;
using Binance.StockExchange.Kline.WS;
using PriceAggregator.Library.GreenRedPercentage;
using PriceAggregator.Library.Percentage;
using PriceAggregator.Library.VolatilityToday;
using PriceAggregator.Library.VolatilityWeekly;
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
        private readonly GreenRedPercentService greenRedPercentService;
        private readonly VolatilityTodayService volatilityTodayService;
        private readonly VolatilityWeeklyService volatilityWeeklyService;

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
            greenRedPercentService = new GreenRedPercentService(repository);
            volatilityTodayService = new VolatilityTodayService(repository);
            volatilityWeeklyService = new VolatilityWeeklyService(repository);

            threadPriceAggregator = new Thread(processing);

            PercentageChanges = new List<PercentageChange>();
            GreenRedPercentChanges = new List<GreenRedPercentChange>();
            VolatilityTodayModels = new List<VolatilityModel>();
            VolatilityWeeklyModels = new List<VolatilityWeeklyModel>();
        }

        public IEnumerable<string> Pairs { get; private set; }
        public List<PercentageChange> PercentageChanges { get; private set; }
        public List<GreenRedPercentChange> GreenRedPercentChanges { get; private set; }
        public List<VolatilityModel> VolatilityTodayModels { get; private set; }
        public List<VolatilityWeeklyModel> VolatilityWeeklyModels { get; private set; }

        public async Task RunAsync(IEnumerable<string> simbols, IEnumerable<string> intervals)
        {
            var tasks = new List<Task>();
            tasks.Add(klineStreamManager.ConnectStreams(simbols, KlineTimeframe.TimeframesAdaptive, CommonSettings.RESTART_STREAM_TIME, CommonSettings.INTERVAL_CHANNEL_RESTART));
            tasks.Add(klineReceiver.Get(simbols, KlineTimeframe.TimeframesAdaptive));
            await Task.WhenAll(tasks);

            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.BackgroundColor = ConsoleColor.Yellow;
            //Console.WriteLine($"{DateTime.Now} Каналы подключены. Исторя загружена.");
            //Console.ResetColor();

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
                GreenRedPercentChanges = greenRedPercentService.GetPercentages(simbols, intervals);
                VolatilityTodayModels = volatilityTodayService.GetVolatilites(simbols);
                VolatilityWeeklyModels = volatilityWeeklyService.GetVolatilites(simbols);
                Thread.Sleep(100);
            }
        }
    }
}
