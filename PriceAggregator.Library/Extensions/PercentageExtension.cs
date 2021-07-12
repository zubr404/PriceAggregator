using Binance.DataSource.Kline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.Extensions
{
    public static class PercentageExtension
    {
        public static decimal? GetPercentage(this IEnumerable<Candle> candles, int countTake)
        {
            if (candles?.Count() > 0)
            {
                var candlesCloses = candles.Where(x => x.IsClose);
                if (candlesCloses?.Count() > 0)
                {
                    var candlesClosesSort = candlesCloses.OrderByDescending(x => x.TimeOpen).Take(countTake);
                    if (candlesClosesSort.Count() == countTake)
                    {
                        var open = candlesClosesSort.Last().Open;
                        var close = candlesClosesSort.First().Close;
                        var percentage = ((close - open) / open) * 100;
                        return percentage;
                    }
                }
            }
            return null;
        }
    }
}
