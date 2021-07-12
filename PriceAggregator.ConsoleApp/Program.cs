using Binance.DataSource.Kline;
using Binance.StockExchange.Kline.REST;
using Binance.StockExchange.Kline.WS;
using Binance.Common.Kline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using PriceAggregator.Library;

namespace PriceAggregator.ConsoleApp
{
    class Program
    {
        private static IRepository candlesRepository = new CandlesRepository();
        private static KlineReceiver klineReceiver = new KlineReceiver(candlesRepository);
        private static KlineStreamManager klineStreamManager = new KlineStreamManager(candlesRepository);
        private static ExchangeInfo exchangeInfo = new ExchangeInfo();

        private static PriceAggregatorManager priceAggregatorManager = new PriceAggregatorManager(candlesRepository);

        private static IEnumerable<string> pairs;
        private static IEnumerable<string> intervals;

        static void Main(string[] args)
        {
            //Dictionary<string, Dictionary<string, List<K>>> dataSource = new Dictionary<string, Dictionary<string, List<K>>>();

            var timeStart = DateTime.Now;
            Console.WriteLine($"{timeStart} ......START......");

            pairs = new string[] { "BTCUSDT" };
            intervals = new string[] { "3h" };
            //pairs = exchangeInfo.AllPairsMarket.MarketPairs.Select(x => x.Pair);
            //intervals = KlineTimeframe.TimeframesAll;
            var intervalsAdaptive = KlineTimeframe.TimeframesAdaptive;

            klineStreamManager.ConnectStreams(pairs, intervalsAdaptive, CommonSettings.RESTART_STREAM_TIME, CommonSettings.INTERVAL_CHANNEL_RESTART);
            klineReceiver.Get(pairs, intervalsAdaptive).GetAwaiter().GetResult();

            var secondThread = new Thread(priceAggregatorRun);
            secondThread.Start();


            Console.WriteLine($"{DateTime.Now} Press any key for close connects Time start {timeStart}");
            Console.Read();
            secondThread.Abort();
            klineStreamManager.DisconnectStream();

            var resultRepositoryString = new StringBuilder();
            foreach (var item1 in candlesRepository.Get())
            {
                //Console.WriteLine(item1.Key);
                resultRepositoryString.Append($"{item1.Key}\n");
                foreach (var item2 in item1.Value)
                {
                    //Console.WriteLine($"\t{item2.Key}");
                    resultRepositoryString.Append($"\t{item2.Key}\n");
                    foreach (var item3 in item2.Value)
                    {
                        //Console.WriteLine($"\t\t{item3.TimeOpen} {item3.TimeClose} {item3.Simbol} {item3.Interval} {item3.Open} {item3.Close} {item3.High} {item3.Low} {item3.IsClose}");
                        resultRepositoryString.Append($"\t\t{item3.TimeOpen} {item3.TimeClose} {item3.Simbol} {item3.Interval} {item3.Open} {item3.Close} {item3.High} {item3.Low} {item3.IsClose}\n");
                    }
                }
            }
            File.WriteAllText(@"C:\tmp\Klines.txt", resultRepositoryString.ToString());

            Console.WriteLine("Press any key for exit");
            Console.ReadKey();
        }

        private static void priceAggregatorRun()
        {
            while (true)
            {
                priceAggregatorManager.Run(pairs, intervals);
                foreach (var percentageChange in priceAggregatorManager.PercentageChanges)
                {
                    Console.WriteLine($"{DateTime.Now} {percentageChange.Simbol} {percentageChange.Interval} {percentageChange.Percentage}");
                }
                Thread.Sleep(2000);
            }
        }

        private static void TestSocket()
        {
            //try
            //{
            //    var exchangeInfo = new ExchangeInfo();

            //    var channels = new StringBuilder();
            //    var maxCountPairs = 560 / KlineResource.TimeframesAll.Count - 1;
            //    for (int i = 0; i < exchangeInfo.AllPairsMarket.MarketPairs.Count; i++)
            //    {
            //        var pair = exchangeInfo.AllPairsMarket.MarketPairs[i];

            //        foreach (var timeframe in KlineResource.TimeframesAll)
            //        {
            //            var channel = $"{pair.Pair.ToLower()}@kline_{timeframe}/";
            //            channels.Append(channel);
            //        }

            //        if (i > 0 && (i % maxCountPairs == 0 || i == exchangeInfo.AllPairsMarket.MarketPairs.Count - 1))
            //        {
            //            klineStreamServices.Add(new KlineStreamService(channels.Remove(channels.Length - 1, 1).ToString()));
            //            channels.Clear();
            //        }
            //    }

            //    for (int i = 0; i < klineStreamServices.Count; i++)
            //    {
            //        var kss = klineStreamServices[i];
            //        kss.SocketOpen();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"{DateTime.Now} {ex.Message}\n{ex.StackTrace}");
            //}

            //Console.WriteLine($"{DateTime.Now} ......FINISH......");

            //Console.WriteLine("Press any key for close connects");
            //Console.Read();

            //for (int i = 0; i < klineStreamServices.Count; i++)
            //{
            //    var kss = klineStreamServices[i];
            //    kss.SocketClose();
            //}
        }
    }
}