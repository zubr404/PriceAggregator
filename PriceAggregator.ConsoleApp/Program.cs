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

namespace PriceAggregator.ConsoleApp
{
    class Program
    {
        private static CandlesRepository candlesRepository = new CandlesRepository();
        private static KlineReceiver klineReceiver = new KlineReceiver(candlesRepository);
        private static KlineStreamManager klineStreamManager = new KlineStreamManager(candlesRepository);
        private static ExchangeInfo exchangeInfo = new ExchangeInfo();

        static void Main(string[] args)
        {
            //Dictionary<string, Dictionary<string, List<K>>> dataSource = new Dictionary<string, Dictionary<string, List<K>>>();

            var timeStart = DateTime.Now;
            Console.WriteLine($"{timeStart} ......START......");

            var _pairs = new string[] { "BTCUSDT" };
            var _intervals = new string[] { "1m" };
            var pairs = exchangeInfo.AllPairsMarket.MarketPairs.Select(x => x.Pair).Take(300);

            // paging
            //for (int i = 0; i < pairs.Count() - 1; i+=19)
            //{
            //    var pairsPage = pairs.Skip(i).Take(19);
            //    klineReceiver.Get(pairsPage, KlineTimeframe.TimeframesAll).GetAwaiter().GetResult();
            //    klineStreamManager.ConnectStreams(pairsPage, KlineTimeframe.TimeframesAll);
            //}


            klineStreamManager.ConnectStreams(pairs, KlineTimeframe.TimeframesAll);
            klineReceiver.Get(pairs, KlineTimeframe.TimeframesAll).GetAwaiter().GetResult();
            



            Console.WriteLine($"{DateTime.Now} Press any key for close connects Time start {timeStart}");
            Console.Read();

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