using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance.StockExchange.Kline.REST
{
    class UrlCreator
    {
        public static string GetKlineUrl(string simbol, string intreval, int limit)
        {
            return $"https://api3.binance.com/api/v3/klines?symbol={simbol}&interval={intreval}&limit={limit}";
        }

        public static string GetTimeUrl()
        {
            return @"https://api3.binance.com/api/v3/time";
        }
    }
}
