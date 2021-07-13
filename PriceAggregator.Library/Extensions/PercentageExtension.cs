using Binance.DataSource.Kline;
using PriceAggregator.Library.Interfaces;
using PriceAggregator.Library.Percentage;
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

        /// <summary>
        /// Возвращает PercentageChange
        /// </summary>
        /// <param name="percentageModel"></param>
        /// <param name="repository"></param>
        /// <param name="simbol"></param>
        /// <param name="intervalBinance">Базовый интервал из документации Бинанс</param>
        /// <param name="intervalUser">Интервал пользователя</param>
        /// <param name="countTake"></param>
        /// <returns></returns>
        public static PercentageChange GetPercentageChange(this IPercentage percentageModel, IRepository repository, string simbol, string intervalBinance, string intervalUser, int countTake)
        {
            var candles = repository.Get(simbol, intervalBinance);
            var percentage = candles.GetPercentage(countTake);
            return new PercentageChange()
            {
                Interval = intervalUser,
                Percentage = percentage,
                Simbol = simbol
            };
        }
    }
}
