using Binance.Common.Kline;
using Binance.DataSource.Kline;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Binance.StockExchange.Kline.REST
{
    public class KlineReceiver
    {
        public const int TIMEOUT_REQUEST_MS = 1000;
        public const int COUNT_REQUEST_FOR_DELAY = 19;

        private readonly CultureInfo cultureInfo;
        private readonly IRepository repository;

        public KlineReceiver(IRepository repository)
        {
            cultureInfo = new CultureInfo("en-US");
            this.repository = repository;
        }


        public async Task Get(IEnumerable<string> pairs, IEnumerable<string> timeFrames)
        {
            await klinesSave(pairs, timeFrames, CommonSettings.LIMIT_KLINES);
        }

        public async Task Get(IEnumerable<string> pairs, IEnumerable<string> timeFrames, int limitKlines, bool fixedDepthHistory)
        {
            await klinesSave(pairs, timeFrames, limitKlines, fixedDepthHistory);
        }

        private async Task klinesSave(IEnumerable<string> pairs, IEnumerable<string> timeFrames, int limitKlines, bool fixedDepthHistory = true)
        {
            if (pairs?.Count() > 0)
            {
                var tasks = new List<Task<List<Candle>>>();

                var countRequest = 1;
                foreach (var pair in pairs)
                {
                    foreach (var interval in timeFrames)
                    {
                        if (countRequest % COUNT_REQUEST_FOR_DELAY == 0)
                        {
                            Thread.Sleep(TIMEOUT_REQUEST_MS);
                        }
                        tasks.Add(GetKlines(pair, interval, limitKlines));
                        countRequest++;
                    }
                }
                await Task.WhenAll(tasks);

                foreach (var task in tasks)
                {
                    foreach (var candle in task.Result)
                    {
                        repository.CreateOrUpdate(candle, fixedDepthHistory);
                    }
                }
            }
        }

        private int countRequest = 0;
        private async Task<List<Candle>> GetKlines(string simbol, string interval, int limit)
        {
            try
            {
                var result = new List<Candle>();
                var url = UrlCreator.GetKlineUrl(simbol, interval, limit);

                var requester = new PublicRequester();
                var response = await requester.RequestPublicApi(url);

                var klines = JConverter.JsonConver<List<object[]>>(response.ResponseMessage);
                foreach (var k in klines)
                {
                    var kline = new Candle()
                    {
                        TimeOpen = Convert.ToInt64(k[0], cultureInfo),
                        TimeClose = Convert.ToInt64(k[6], cultureInfo),
                        Simbol = simbol,
                        Interval = interval,
                        Open = Convert.ToDecimal(k[1], cultureInfo),
                        Close = Convert.ToDecimal(k[4], cultureInfo),
                        High = Convert.ToDecimal(k[2], cultureInfo),
                        Low = Convert.ToDecimal(k[3], cultureInfo),
                        IsClose = true
                    };
                    result.Add(kline);
                }
                countRequest++;
                Console.WriteLine($"{DateTime.Now} {simbol} {interval} {countRequest}");
                return result;
            }
            catch (Exception)
            {
                return new List<Candle>();
            }
        }
    }
}
