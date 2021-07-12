using Binance.Common.Kline;
using Binance.DataSource.Kline;
using PriceAggregator.Library.Interfaces;
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
    public class PercentageMatchingInterval : IPercentage
    {
        private readonly IRepository repository;
        private readonly string simbol;
        private readonly string interval;

        public PercentageMatchingInterval(IRepository repository, string simbol, string interval)
        {
            this.repository = repository;
            this.simbol = simbol;
            this.interval = interval;
        }

        public PercentageChange GetPercentage()
        {
            var candles = repository.Get(simbol, interval);
            var percentage = getPercentage(candles);
            return new PercentageChange()
            {
                Interval = interval,
                Percentage = percentage.Value,
                Simbol = simbol
            };
        }

        private decimal? getPercentage(IEnumerable<Candle> candles)
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
