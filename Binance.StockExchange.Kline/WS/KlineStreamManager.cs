using Binance.DataSource.Kline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.StockExchange.Kline.WS
{
    public class KlineStreamManager
    {
        private const int MAX_COUNT_PAIRS = 37;
        private readonly List<KlineStreamService> klineStreamServices;
        private readonly IRepository repository;

        public KlineStreamManager(IRepository repository)
        {
            this.repository = repository;
            klineStreamServices = new List<KlineStreamService>();
        }

        public void ConnectStreams(IEnumerable<string> pairs, IEnumerable<string> timeFrames, TimeSpan restartSocketTime, TimeSpan intervalChannelRestart)
        {
            if (pairs?.Count() > 0)
            {
                if (timeFrames?.Count() > 0)
                {
                    var restartTime = restartSocketTime;
                    var pairsList = pairs.ToList();
                    var klineServices = new List<KlineStreamService>();
                    try
                    {
                        var channels = new StringBuilder();
                        for (int i = 0; i < pairsList.Count; i++)
                        {
                            var pair = pairsList[i];

                            foreach (var timeframe in timeFrames)
                            {
                                var channel = $"{pair.ToLower()}@kline_{timeframe}/";
                                channels.Append(channel);
                            }

                            if (i > 0 && i % MAX_COUNT_PAIRS == 0 || i == pairsList.Count - 1)
                            {
                                restartTime = restartTime.Add(intervalChannelRestart);
                                var klineService = new KlineStreamService(repository, channels.Remove(channels.Length - 1, 1).ToString(), restartTime);
                                klineStreamServices.Add(klineService);
                                klineServices.Add(klineService);
                                channels.Clear();
                            }
                        }

                        for (int i = 0; i < klineServices.Count; i++)
                        {
                            var kss = klineStreamServices[i];
                            kss.SocketOpen();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{DateTime.Now} {ex.Message}\n{ex.StackTrace}");
                    }
                }
            }
        }

        public void DisconnectStream()
        {
            for (int i = 0; i < klineStreamServices.Count; i++)
            {
                var kss = klineStreamServices[i];
                kss.SocketClose();
            }
        }
    }
}
