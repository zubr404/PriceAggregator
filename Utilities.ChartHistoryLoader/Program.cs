using Binance.DataSource.Kline;
using Binance.StockExchange.Kline.REST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.ChartHistoryLoader
{
    class Program
    {
        private static IEnumerable<string> pairs;
        private static IRepository candlesRepository;
        private static KlineReceiver klineReceiver;

        static void Main(string[] args)
        {
            Console.WriteLine($"{DateTime.Now} History loading started");
            Console.WriteLine($"Please specify the path to the folder to save the data (for example, C:\\CandlesHistory):");
            var rootFolderPath = Console.ReadLine();
            createFolder(rootFolderPath);

            try
            {
                var exchangeInfo = new ExchangeInfo();
                pairs = exchangeInfo.AllPairsMarket.MarketPairs.Select(x => x.Pair).Where(x => x.Contains("BTC") || x.Contains("ETH"));

                //pairs = new string[] { "BTCUSDT"/*, "ETHUSDT"*/ };

                Console.WriteLine($"{DateTime.Now} Currency pairs were obtained. Quantity - {pairs.Count()}");

                Console.WriteLine($"{DateTime.Now} I'm starting to receive data from the exchange");
                var intervals = new string[] { "1h", "6h", "12h", "1w" }; // 1 hour, 6 hour, 12 hour, 1 day and 1 week

                candlesRepository = new CandlesRepository();
                klineReceiver = new KlineReceiver(candlesRepository);
                klineReceiver.Get(pairs, intervals, 1000, false).GetAwaiter().GetResult();

                Console.WriteLine($"{DateTime.Now} The data from the exchange was successfully received. I save the data to files");
                var currentDateTimeFolderPath = Path.Combine(rootFolderPath, DateTime.Now.ToString("yyyy.MM.dd HH_mm_ss"));
                createFolder(currentDateTimeFolderPath); // создаем папку текущего времени

                //--------------------------------------------------------------
                foreach (var interval in intervals)
                {
                    var dataString = new StringBuilder();

                    // набираем времЕнные столбцы
                    var unixTimes = getTimeCells(interval, dataString);

                    foreach (var pairAllIntervalCandles in candlesRepository.Get()) // pairAllIntervalCandles - все интервалы по одной паре
                    {
                        var candles = pairAllIntervalCandles.Value[interval].OrderByDescending(x => x.TimeOpen); // candles - свечи по  одной паре по интервалу interval

                        //foreach (var candle in candles)
                        //{
                        //    Console.WriteLine(candle.TimeOpen.UnixToDateTime());
                        //}


                        var rowTable = new StringBuilder();
                        rowTable.Append(pairAllIntervalCandles.Key); // название пары

                        foreach (var time in unixTimes)
                        {
                            var candle = candles.FirstOrDefault(x => x.TimeOpen == time);
                            if (candle != null)
                            {
                                var percentage = Math.Round(((candle.Close - candle.Open) / candle.Open) * 100, 2);
                                rowTable.Append($"\t{percentage}");
                            }
                            else
                            {
                                rowTable.Append($"\tNULL");
                            }
                        }
                        rowTable.Append("\n");
                        dataString.Append(rowTable);
                    }
                    var csvFileFullPath = Path.Combine(currentDateTimeFolderPath, $"{interval}.csv");
                    File.WriteAllText(csvFileFullPath, dataString.ToString());
                }
            }
            catch (Exception ex)
            {
                messageError(ex.Message);
            }
            
            Console.WriteLine($"{DateTime.Now} History loading is complete");
            Console.Read();
        }

        private static List<long> getTimeCells(string interval, StringBuilder dataString)
        {
            long intervalMillisecond = 1;
            switch (interval)
            {
                case "1h":
                    intervalMillisecond = 3600000;
                    break;
                case "6h":
                    intervalMillisecond = 21600000;
                    break;
                case "12h":
                    intervalMillisecond = 43200000;
                    break;
                case "1w":
                    intervalMillisecond = 604800000;
                    break;
                default:
                    break;
            }
            var times = new List<long>();
            long unixTimeInterval;
            if (interval == "1w")
            {
                var today = DateTime.UtcNow.Date;
                for (int i = 0; i < 7; i++)
                {
                    if (today.DayOfWeek != DayOfWeek.Monday)
                    {
                        today = today.AddDays(-1);
                    }
                    else
                    {
                        break;
                    }
                }
                unixTimeInterval = today.ToUnixTime();
            }
            else
            {
                var unixTime = DateTime.UtcNow.ToUnixTime();
                var remainderUnix = unixTime % intervalMillisecond;
                unixTimeInterval = unixTime - remainderUnix;
            }

            for (int i = 1; i <= 1000; i++)
            {
                times.Add(unixTimeInterval);
                dataString.Append($"\t{unixTimeInterval.UnixToDateTime()}"); // шапка
                unixTimeInterval -= intervalMillisecond;
            }
            dataString.Append("\n");

            return times;
        }

        private static bool createFolder(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void messageError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now} {message}");
            Console.ResetColor();
        }
    }
}
