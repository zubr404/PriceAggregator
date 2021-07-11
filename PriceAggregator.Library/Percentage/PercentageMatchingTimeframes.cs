using Binance.DataSource.Kline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregator.Library.Percentage
{
    /// <summary>
    /// Вычисление процентов для таймфреймов, которые совпадают с документацией Бинанс
    /// </summary>
    class PercentageMatchingTimeframes
    {
        public static decimal? GetPercentage(IEnumerable<Candle> candles)
        {
            if (candles?.Count() > 0)
            {
                var candlesCloses = candles.Where(x => x.IsClose);
                if (candlesCloses?.Count() > 0)
                {
                    var candleLast = candlesCloses.First(x => x.TimeOpen == candlesCloses.Max(t => t.TimeOpen));
                    var percentage = ((candleLast.Close - candleLast.Open) / candleLast.Open) * 100;
                    return percentage;
                }
            }
            return null;
        }
    }
}
